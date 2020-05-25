using System;
using System.Data;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Models.Processing
{
    public class Trainer : IProcessor
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
                if (!long.TryParse(row["FILE_ID"].ToString(), out long FID))
                    throw new Exception("Error getting files!");

                file = new File(FID);
              
                Analyze();                               
            }

            isSuccesful = true;
        }

        /// <summary>
        /// Initiates analysis process
        /// </summary>
        private void Analyze()
        {
            if (file.ExistsProp)
            {
                //static analysis
                ExtractPackageName();
                ExtractPermissions();
                ExtractIntent();
                //Decompile();
                //GetApiCalls();

                //dynamic analysis
                t1 = new Terminal();
                ConnectDevice();
                InstallSample();
                ExtractDynamicData();
                DisconnectDevice();
                t1.dispose();
            }
            else
                throw new Exception("File does not exists");

            analysisSuccesful = true;
        }

    }
}
