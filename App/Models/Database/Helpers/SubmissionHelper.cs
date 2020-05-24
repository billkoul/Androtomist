using System;
using System.Collections.Generic;
using System.Data;
using Androtomist.Models.Database.Inserters;
using Androtomist.Models.Forms;

namespace Androtomist.Models.Database.Helpers
{
    public class SubmissionHelper : AbstractHelper
    {
        public void RemoveSubmission(List<long> BOTTLE_IDS)
        {
            int rowsAffected = 0;

            rowsAffected = databaseConnector.DeleteSQL("DELETE FROM B_SUBS WHERE SUB_ID IN(" + string.Join(",", BOTTLE_IDS) + ")");

            if (rowsAffected == 0)
                throw new Exception("Could not remove any records from database.");

            if (rowsAffected != BOTTLE_IDS.Count)
                throw new Exception("Could not remove " + (BOTTLE_IDS.Count - rowsAffected) + " records from database.");
        }
    }


}