using System;
using System.Collections.Generic;
using System.Data;
using Androtomist.Models.Database.Inserters;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Helpers
{
    public class ProcessHelper : AbstractHelper
    {
		public Entities.Process AddProcess(string createJson)
		{
			FilterData filterData = new FilterData(createJson);

			long.TryParse(filterData.P_ID, out long ID);
            if (string.IsNullOrEmpty(filterData.P_NAME)) throw new Exception("Please add a name for process!");

            //comment out the following line to allow dynamic analysis
            if (filterData.P_TYPE_ID != "0")
                return null;

            if (ID > 0)
                databaseConnector.DeleteSQL("DELETE FROM C_PROCESS WHERE P_ID = " + ID);

            ProcessInserter processInserter = new ProcessInserter
            {
				P_ID = (ID < 0 ? null : ID.ToString()),
                P_TYPE_ID = filterData.P_TYPE_ID,
                P_NAME = filterData.P_NAME,
                P_FILE_ID = filterData.FILE_ID
            };
			ID = processInserter.Insert(true);

			Entities.Process process = new Entities.Process(ID);
			if (!process.Exists()) throw new Exception("Cannot add process to database");

			return process;
		}

        public void RemoveProcess(List<long> IDS)
        {
            GeneralPriviledgesCheker gp = new GeneralPriviledgesCheker();
            if(!gp.IsPowerfull())
                throw new Exception("You dont have permission to delete this item!");

            int rowsAffected = databaseConnector.DeleteSQL("DELETE FROM C_PROCESS WHERE P_ID IN(" + string.Join(",", IDS) + ")");
            if (rowsAffected == 0)
                throw new Exception("Could not remove any records from database.");

            if (rowsAffected != IDS.Count)
                throw new Exception("Could not remove " + (IDS.Count - rowsAffected) + " records from database.");
        }
	}
}