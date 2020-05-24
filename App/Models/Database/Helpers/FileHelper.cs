using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Androtomist.Models.Database.Entities;
using System.IO;
using Androtomist.Models.Database.Inserters;
using Microsoft.AspNetCore.Http;

namespace Androtomist.Models.Database.Helpers
{
	public class FileHelper : AbstractHelper
	{
		public void RemoveFiles(long FILE_ID)
		{
			RemoveFiles(new long[] { FILE_ID }.ToList());
		}

		public void RemoveFiles(List<long> FILE_IDS)
		{
			FileFuncs fileFuncs = new FileFuncs();
			int rowsAffected = 0;
			DataTable dataTable;
			string SQL;

			SQL = "SELECT FILE_ID FROM A_FILE WHERE FILE_ID IN(" + string.Join(",", FILE_IDS) + ")";
			dataTable = databaseConnector.SelectSQL(SQL, "A_FILE");
			List<long> FILE_IDS_USED = dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt64(x["FILE_ID"])).ToList();

			FILE_IDS_USED.Select(x => new Androtomist.Models.Database.Entities.File(x)).Where(x => x.Exists()).ToList().ForEach(x => fileFuncs.FileDelete(x.FILE_PATH));

			rowsAffected = databaseConnector.DeleteSQL("DELETE FROM A_FILE WHERE FILE_ID IN(" + string.Join(",", FILE_IDS) + ")");
			//cascade or rowsAffected = databaseConnector.DeleteSQL("DELETE FROM B_SUBFILE WHERE SUBFILE_FILE_ID IN(" + string.Join(",", FILE_IDS) + ")");

			if (rowsAffected == 0)
				throw new Exception("Could not remove any records from database.");

			if (rowsAffected != FILE_IDS.Count)
				throw new Exception("Could not remove " + (FILE_IDS.Count - rowsAffected) + " records from database.");

		}
		private string GetWebRootPath()
		{
			return System.Web.HostingEnvironment.Current.WebRootPath;
		}

		public string GetUploadPath(bool create = false, string extra_path = "")
		{
			FileFuncs fileFuncs = new FileFuncs();
			string uploadPath;

			fileFuncs.FolderClearOldFiles(Path.Combine(GetWebRootPath(), "files", "uploads"));

			if (!string.IsNullOrEmpty(extra_path))
				uploadPath = Path.Combine(GetWebRootPath(), "files", "uploads", extra_path);
			else
				uploadPath = Path.Combine(GetWebRootPath(), "files", "uploads");

			if (create) fileFuncs.FolderCreate(uploadPath);

			return uploadPath;
		}
	}
}
