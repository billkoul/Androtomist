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
    public class Trainer : DBClass
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


        public Trainer(string goodware, string malware)
        {
            //read information from a_info and add to Info object
            Info = new Info(0);

            sampleDt = databaseConnector.SelectSQL("select file_id from a_file where FILE_LABEL = :param1 or FILE_LABEL = :param2", "a_file", goodware, malware);
            //update label to be used in signature based detection views
            databaseConnector.UpdateSQL("update a_info set goodware_file_label = :param1, malware_file_label = :param2", "a_file", goodware, malware);

        }

        /// <summary>
        /// Initiates training process
        /// </summary>
        public void Train()
        {
            foreach (DataRow row in sampleDt.Rows)
            {
                t1 = new Terminal(true);

                if (!long.TryParse(row["FILE_ID"].ToString(), out long FID))
                    throw new Exception("Error getting files!");

                file = new File(FID);

                ConnectDevice();
                InstallSample();
                Analyze();
                DisconnectDevice();
                t1.dispose();
            }

            isSuccesful = true;
        }

        /// <summary>
        /// Connects VM with ADB and Frida
        /// </summary>
        private void ConnectDevice()
        {
            if (file.ExistsProp)
            {
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ;.\\adb connect " + Info.REMOTE_ADDR);
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ;.\\adb push ../frida-server-12.6.23-android-x86_64 /data/local/tmp");
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ;.\\adb shell \"su -c 'chmod 755 /data/local/tmp/frida-server-12.6.23-android-x86_64'\";");
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ;.\\adb shell \"su -c '/data/local/tmp/frida-server-12.6.23-android-x86_64 >/dev/null 2>&1 &'\";");
            }
            else
                throw new Exception("File does not exists");

            preperationSuccesful = true;
        }

        /// <summary>
        /// Disconnects the device
        /// </summary>
        private void DisconnectDevice()
        {
            if (file.ExistsProp) 
                t1.exec("cd " + Info.TOOLS_PATH + " ;.\\adb disconnect " + Info.REMOTE_ADDR);
            else
                throw new Exception("File does not exists");

            preperationSuccesful = true;
        }


        /// <summary>
        /// Installs sample to VM
        /// </summary>
        private void InstallSample()
        {
            if (file.ExistsProp)
            {
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ; .\\adb push '" + Info.PROJECT_PATH + "" + file.RELATIVE_PATH + "' /data/local/tmp;");
                _ = t1.exec("cd " + Info.TOOLS_PATH + " ; .\\adb shell 'cd /data/local/tmp; pm install " + file.ORIGINAL_FILE_NAME.Replace("(", "\\(").Replace(")","\\)") + "';");
            }
            else
                throw new Exception("File does not exists");

            installSuccesful = true;
        }

        /// <summary>
        /// Initiates analysis process
        /// </summary>
        private void Analyze()
        {
            if (file.ExistsProp)
            {
                //Analysis steps
                ExtractPackageName();
                ExtractPermissions();
                ExtractIntent();
                Decompile();
                GetApiCalls();
                ExtractDynamicData();
            }
            else
                throw new Exception("File does not exists");

            analysisSuccesful = true;
        }

        /// <summary>
        /// Reads package name
        /// </summary>
        private void ExtractPackageName()
        {
            string package = string.Join("", t1.cmd("cd /d " + Info.TOOLS_PATH + " && aapt.exe  dump badging " + file.FILE_PATH + ""));

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

                databaseConnector.UpdateSQL("A_FILE_TRAIN", colNames, col_vals, "FILE_ID=" + file.FILE_ID);
            }
        }

        /// <summary>
        /// Reads permissions
        /// </summary>
        private void ExtractPermissions()
        {
            string permissionText = string.Join("", t1.cmd("cd /d " + Info.TOOLS_PATH + " && aapt.exe dump permissions " + file.FILE_PATH + ""));

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

                    databaseConnector.DeleteSQL("DELETE FROM R_PERMISSIONS WHERE FILE_ID = " + file.FILE_ID);

                    databaseConnector.InsertSQL("R_PERMISSIONS", col_names, col_vals);
                }
            }
        }
        private bool IsValidJson(string strInput)
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

        /// <summary>
        /// Reads intents
        /// </summary>
        private void ExtractIntent()
        {
            string intentFilter = string.Join("", t1.cmd("cd /d " + Info.TOOLS_PATH + " && aapt.exe dump xmltree " + file.FILE_PATH + " AndroidManifest.xml"));
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

        private void Decompile()
        {
            t1.cmd("java -jar " + Info.TOOLS_PATH + "\\apktool.jar d -o " + Info.PROJECT_PATH + "\\files\\decoded\\" + file.ORIGINAL_FILE_NAME + " -f " + file.FILE_PATH);
        }

        /// <summary>
        /// Get API Calls
        /// </summary>
        private void GetApiCalls()
        {
            //not implemented in this version
        }

        /// <summary>
        /// Initiates dynamic analysis with Frida
        /// </summary>
        private void ExtractDynamicData()
        {
            //init file_id to DB
            List<string> colNames = new List<string>{
                    "FILE_ID"
            };

            List<string> colVals = new List<string> {
                    "'" + file.FILE_ID + "'"
            };

            //databaseConnector.InsertSQL("A_TRAIN_DI", colNames, colVals);

            string data = "";

            File fileNew = new File(file.FILE_ID);

            if (string.IsNullOrEmpty(fileNew.PACKAGE_NAME))
                return;

            #region ParallelTasks
            Parallel.Invoke (
                () =>
                {
                    data += t1.cmd("cd C:\\Users\\billkoul\\Downloads\\AndroidLab\\frida && frida -U -l " + Info.SCRIPT + " " + fileNew.PACKAGE_NAME, 60000);
                },
                () =>
                {
                    t1.cmd("cd C:\\Users\\billkoul\\Downloads\\AndroidLab\\platform-tools && .\\adb shell \"monkey -p " + fileNew.PACKAGE_NAME + " --pct-trackball 0 --pct-syskeys 0 --pct-nav 0 --pct-majornav 0 --ignore-crashes -v " + Info.EVENTS + "\"", 5000);
                },
                () =>
                {
                    data += t1.cmd("cd C:\\Users\\billkoul\\Downloads\\AndroidLab\\frida && frida -U -l " + Info.SCRIPT + " " + fileNew.PACKAGE_NAME,60000);
                }
            );
            //scan.js => traffic
            //intent4.js => intent
            #endregion

            System.Threading.Thread.Sleep(5000);

            if (!string.IsNullOrEmpty(data))
            {
                colNames = new List<string>{
                    "FILE_ID"
                };
                if (!string.IsNullOrEmpty(data)) colNames.Add("TRAFFIC");

                colVals = new List<string> {
                    "'" + fileNew.FILE_ID + "'"
                };
                if (!string.IsNullOrEmpty(data)) colVals.Add("'" + (data.IndexOf("Attaching") != -1 ? data.Substring(data.IndexOf("Attaching")) : data) + "'");

                databaseConnector.UpdateSQL("A_TRAIN_DI", colNames, colVals, "FILE_ID="+ fileNew.FILE_ID);

                return;
            }
        }
    }
}
