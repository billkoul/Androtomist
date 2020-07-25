using System;
using System.Data;
using System.Collections.Generic;
using Androtomist.Models.Database;
using Androtomist.Models.Database.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Androtomist.Models.Results;
using Newtonsoft.Json;

namespace Androtomist.Models.Processing
{
    public abstract class IProcessor : DBClass
    {
        private Terminal t1;
        private File file;
        public bool isSuccesful = false;
        public bool installSuccesful = false;
        public bool analysisSuccesful = false;
        public bool preperationSuccesful = false;
        readonly DataTable sampleDt;
        private readonly Info Info;
        private string packageName;


        #region PREPARE DEVICE

        protected void ConnectDevice()
        {
            if (file.ExistsProp)
            {
                _ = t1.cmd("cd /d " + Info.TOOLS_PATH + " && adb.exe connect " + Info.REMOTE_ADDR);
                _ = t1.cmd("cd /d " + Info.TOOLS_PATH + " && adb.exe push ../frida-server-12.6.23-android-x86_64 /data/local/tmp");
                _ = t1.cmd("cd /d " + Info.TOOLS_PATH + " && adb.exe shell \"su -c 'chmod 755 /data/local/tmp/frida-server-12.6.23-android-x86_64'\"");
                _ = t1.cmd("cd /d " + Info.TOOLS_PATH + " && adb.exe shell \"su -c '/data/local/tmp/frida-server-12.6.23-android-x86_64 >/dev/null 2>&1 &'\"");
            }
            else
                throw new Exception("File does not exists");

            preperationSuccesful = true;
        }

        protected void DisconnectDevice()
        {
            if (file.ExistsProp)
                t1.cmd("cd /d " + Info.TOOLS_PATH + " && adb.exe disconnect " + Info.REMOTE_ADDR);
            else
                throw new Exception("File does not exists");

            preperationSuccesful = true;
        }

        protected void InstallSample()
        {
            if (file.ExistsProp)
            {
                _ = t1.cmd("cd " + Info.TOOLS_PATH + " && adb.exe push " + file.FILE_PATH + " /data/local/tmp");
                _ = t1.cmd("cd " + Info.TOOLS_PATH + " && adb.exe shell \"cd /data/local/tmp; pm install " + file.ORIGINAL_FILE_NAME.Replace("(", "\\(").Replace(")", "\\)") + "\"");
            }
            else
                throw new Exception("File does not exists");

            installSuccesful = true;
        }
        #endregion

        #region STATIC ANALYSIS CONTROLLER

        protected void ExtractPackageName()
        {
            string package = string.Join("", t1.cmd("cd " + Info.TOOLS_PATH + " && aapt.exe  dump badging " + file.FILE_PATH + ""));

            int pack_start = package.IndexOf("package: name='");
            string package_name = package.Substring(pack_start + 15);
            package_name = package_name.Substring(0, package_name.IndexOf("'"));

            if (package_name.Length > 0)
            {
                List<string> colNames = new List<string>{
                        "package_name"
                };

                List<string> col_vals = new List<string> {
                        "'" + package_name + "'"
                };

                databaseConnector.UpdateSQL("A_FILE", colNames, col_vals, "FILE_ID=" + file.FILE_ID);
            }
        }
        
        protected void Decompile()
        {
            t1.cmd("java -jar " + Info.TOOLS_PATH + "\\apktool.jar d -o " + Info.PROJECT_PATH + "\\files\\decoded\\" + file.ORIGINAL_FILE_NAME + " -f " + file.FILE_PATH);
        }

        protected void ExtractPermissions()
        {
            /*
            // XML Parsing
            var xmlParser = new XmlParser(Info.PROJECT_PATH + "\\files\\decoded\\" + file.ORIGINAL_FILE_NAME + "\\Manifest.xml");
            List<string> permissions = xmlParser.GetPermissions();
            
            foreach (string permission in permissions)
            {
                DataTable dt = databaseConnector.SelectSQL("SELECT ID FROM A_PERMISSION WHERE VALUE LIKE '" + permission + "'", "A_PERMISSION");
                int permission_id = (dt2.Rows.Count > 0 ? Convert.ToInt32(dt2.Rows[0]["ID"]) : -1);

                List<string> col_names = new List<string>{
                        "FILE_ID",
                        "PERMISSION",
                        "PERMISSION_ID"
                };

                List<string> col_vals = new List<string> {
                        "'" + file.FILE_ID + "'",
                        "'" + permission + "'",
                        "'" + permission_id + "'"
                };

                databaseConnector.DeleteSQL("DELETE FROM R_PERMISSIONS WHERE FILE_ID = " + file.FILE_ID);
                databaseConnector.InsertSQL("R_PERMISSIONS", col_names, col_vals);
            }
            */
            string permissionText = string.Join("", t1.cmd("cd " + Info.TOOLS_PATH + " && aapt.exe dump permissions " + file.FILE_PATH + ""));

            PermissionParser pp = new PermissionParser();
            string json = pp.ParsePermissionJson(permissionText, file.PACKAGE_NAME);

            if (json.Length > 0)
            {
                databaseConnector.DeleteSQL("DELETE FROM R_STATIC WHERE FILE_ID = " + file.FILE_ID);

                List<string> colNames = new List<string>{
                    "file_id",
                    "permission_json"
                };

                List<string> col_vals = new List<string> {
                    "'" + file.FILE_ID + "'",
                    "'" + json + "'"
                };

                databaseConnector.InsertSQL("R_STATIC", colNames, col_vals);
            }

            string permissions = json.Replace(System.Environment.NewLine, "").Replace(" ", "").Replace("\".", "\"").Replace("N]}", "\"]}").Replace("t]}", "\"]}");
            permissions = Regex.Replace(permissions, @"\t|\n|\r", "");

            if (IsValidJson(permissions))
            {
                APK app = JsonConvert.DeserializeObject<APK>(permissions);

                databaseConnector.DeleteSQL("DELETE FROM R_PERMISSIONS WHERE FILE_ID = " + file.FILE_ID);
                foreach (string permission in app.permissions)
                {
                    DataTable dt2 = databaseConnector.SelectSQL("SELECT ID FROM A_PERMISSION WHERE VALUE LIKE '" + permission + "'", "A_PERMISSION");
                    int permission_id = (dt2.Rows.Count > 0 ? Convert.ToInt32(dt2.Rows[0]["ID"]) : -1);

                    List<string> col_names = new List<string>{
                            "FILE_ID",
                            "PERMISSION",
                            "PERMISSION_ID"
                        };

                    List<string> col_vals = new List<string> {
                            "'" + file.FILE_ID + "'",
                            "'" + permission + "'",
                            "'" + permission_id + "'"
                        };
                    databaseConnector.InsertSQL("R_PERMISSIONS", col_names, col_vals);
                }
            }
        }
        protected bool IsValidJson(string strInput)
        {
            strInput = strInput.Replace("[\",", "[");
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch
                {
                    //Exception in parsing json
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected void ExtractIntent()
        {
            /*
            // XML Parsing
            var xmlParser = new XmlParser(Info.PROJECT_PATH + "\\files\\decoded\\" + file.ORIGINAL_FILE_NAME + "\\Manifest.xml");
            List<string> intents = xmlParser.GetIntents();
            
            databaseConnector.DeleteSQL("DELETE FROM R_INTENT WHERE FILE_ID = " + file.FILE_ID);
            foreach (string intent in intents)
            {
                List<string> col_names = new List<string>{
                        "FILE_ID",
                        "INTENT",
                };

                List<string> col_vals = new List<string> {
                        "'" + file.FILE_ID + "'",
                        "'" + intent + "'",
                };

                databaseConnector.InsertSQL("R_INTENT", col_names, col_vals);
            }
            */
            string intentFilter = string.Join("", t1.cmd("cd " + Info.TOOLS_PATH + " && aapt.exe dump xmltree " + file.FILE_PATH + " AndroidManifest.xml"));
            string json = "";

            if (intentFilter.IndexOf("intent-filter") > -1)
            {
                IntentParser ip = new IntentParser();
                json = ip.ParseIntentJson(intentFilter);

                if (json.Length > 0)
                {
                    List<string> colNames = new List<string>{
                        "FILE_ID",
                        "INTENT_FILTER"
                    };

                    List<string> colVals = new List<string> {
                        "'" + file.FILE_ID + "'",
                        "'" + json + "'"
                    };
                    databaseConnector.InsertSQL("R_STATIC", colNames, colVals);
                }
            }

            var cleanJson = json.Replace("$", "").Replace(":\"", "\",\"").Replace(System.Environment.NewLine, "");
            if (IsValidJson(cleanJson))
            {

                APK app = JsonConvert.DeserializeObject<APK>(cleanJson);

                databaseConnector.DeleteSQL("DELETE FROM R_INTENT WHERE FILE_ID = " + file.FILE_ID);
                foreach (string intent in app.intent)
                {
                    List<string> col_names = new List<string>{
                            "FILE_ID",
                            "INTENT",
                        };

                    List<string> col_vals = new List<string> {
                            "'" + file.FILE_ID + "'",
                            "'" + intent + "'",
                        };

                    databaseConnector.InsertSQL("R_INTENT", col_names, col_vals);
                }
            }
        }

        protected void GetAPICalls()
        {
            string json = t1.cmd("py " + Info.TOOLS_PATH + "\\APITracer.py " + Info.PROJECT_PATH + "\\files\\decoded\\" + file.ORIGINAL_FILE_NAME);
            
            if (IsValidJson(json))
            {
                List<string> col_names = new List<string>{
                        "FILE_ID",
                        "API_CALLS_JSON",
                };

                List<string> col_vals = new List<string> {
                        "'" + file.FILE_ID + "'",
                        "'" + json + "'",
                };

                databaseConnector.InsertSQL("R_STATIC", col_names, col_vals);
                
            }
        }

        #endregion

        #region DYNAMIC INSTRUMENTATION CONTROLLER

        protected void ExtractDynamicData()
        {
            string data = "";

            #region ParallelTasks
            Parallel.Invoke(
                () =>
                {
                    data += t1.cmd("cd " + Info.FRIDA_PATH + " && frida -U -l " + Info.SCRIPT + " " + file.PACKAGE_NAME, 60000);
                },
                () =>
                {
                    t1.cmd("cd " + Info.TOOLS_PATH + " && adb.exe shell \"monkey -p " + file.PACKAGE_NAME + " --pct-trackball 0 --pct-syskeys 0 --pct-nav 0 --pct-majornav 0 --ignore-crashes -v " + Info.EVENTS + "\"", 5000);
                },
                () =>
                {
                    data += t1.cmd("cd " + Info.FRIDA_PATH + " && frida -U -l " + Info.SCRIPT + " " + file.PACKAGE_NAME, 60000);
                }
            );
            #endregion

            if (!string.IsNullOrEmpty(data))
            {
                List<string> colNames = new List<string>{
                    "FILE_ID"
                };
                if (!string.IsNullOrEmpty(data)) colNames.Add("DATA");

                List<string> colVals = new List<string> {
                    "'" + file.FILE_ID + "'"
                };
                if (!string.IsNullOrEmpty(data)) colVals.Add("'" + (data.IndexOf("Attaching") != -1 ? data.Substring(data.IndexOf("Attaching")) : data) + "'");

                databaseConnector.InsertSQL("R_DI", colNames, colVals);

                return;
            }
            else
            {
                List<string> colNames = new List<string>{
                    "FILE_ID"
                };

                List<string> colVals = new List<string> {
                    "'" + file.FILE_ID + "'"
                };

                databaseConnector.InsertSQL("R_DI", colNames, colVals);
                return;
            }
        }

        #endregion
    }
}
