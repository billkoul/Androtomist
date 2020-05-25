using System;
using Androtomist.Models.Database.Entities;

namespace Androtomist.Models.Processing
{
    public class Analyzer : IProcessor
    {
        Process p;
        private Terminal t1;
        private File file;
        public bool isSuccesful = false;
        public bool installSuccesful = false;
        public bool preperationSuccesful = false;
        private readonly Info Info;

        public Analyzer(Process p)
        {
            Info = new Info(0);
            this.p = p;

            file = new File(p.P_FILE_ID);
        }

        /// <summary>
        /// Initiates analysis process
        /// </summary>
        public long Analyze()
        {
            //comment out the following line to allow dynamic analysis
            if ((PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.DYNAMIC || (PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.HYBRID)
                throw new Exception("This action is not permitted with a demo account");

            if (file.ExistsProp)
            {
                if ((PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.STATIC || (PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.HYBRID)
                {
                    ExtractPackageName();
                    ExtractPermissions();
                    ExtractIntent();
                    //Decompile();
                    //GetApiCalls
                }
                if ((PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.DYNAMIC || (PROCESS_TYPE)p.P_TYPE_ID == PROCESS_TYPE.HYBRID)
                {
                    t1 = new Terminal(true);
                    ConnectDevice();
                    InstallSample();
                    ExtractDynamicData();
                    t1.dispose();
                }
            }
            else
                throw new Exception("File does not exists");

            isSuccesful = true;

            return file.FILE_ID;
        }
    }
}
