using System;
using System.IO;
using Androtomist.Models.Files;

namespace Androtomist.Models.Database.Entities
{
    public class File : EntityAbstract
    {
        private FileFuncs fileFuncs;

        public File(long FILE_ID)
        {
            GetRow(@"
                SELECT 
                    *

                FROM A_FILE

                WHERE 
                    A_FILE.FILE_ID = " + FILE_ID + @"
               
                ");

            fileFuncs = new FileFuncs();
        }
		public bool ExistsProp
		{
			get
			{
				return dataRow != null;
			}
		}

		public long FILE_ID
        {
            get
            {
                return Convert.ToInt64(dataRow["FILE_ID"]);
            }
        }

        public DateTime DATE_CREATED
        {
            get
            {
                return Convert.ToDateTime(dataRow["TIME_STAMP"]);
            }
        }

        public string ORIGINAL_FILE_NAME
        {
            get
            {
                return Convert.ToString(dataRow["FILE_NAME"]);
            }
        }

        public string RELATIVE_PATH
        {
            get
            {
                return Convert.ToString(dataRow["FILE_RELATIVE_PATH"]).Replace("\n","");
            }
        }

        public string FILE_PATH
        {
			get
			{
				return Convert.ToString(dataRow["FILE_PATH"]);
			}
		}

        public string ABSOLUTE_PATH
        {
            get
            {
                UploadPathHelper uploadPathHelper = new UploadPathHelper();

                if (!System.IO.File.Exists(FILE_PATH)) throw new Exception("File is missing from filesystem");

                string ABSOLUTE_FILE_PATH = Path.Combine(uploadPathHelper.GetUploadPath(true, Path.GetFileName(FILE_PATH)), ORIGINAL_FILE_NAME);
                if (!System.IO.File.Exists(ABSOLUTE_FILE_PATH)) fileFuncs.FileUnzip(FILE_PATH, Path.GetDirectoryName(ABSOLUTE_FILE_PATH));

                if (FILE_TYPE == FILE_TYPES.zip) fileFuncs.FileUnzip(ABSOLUTE_FILE_PATH, Path.GetDirectoryName(ABSOLUTE_FILE_PATH));

                return ABSOLUTE_FILE_PATH;
            }
        }

        public bool FILE_EXISTS
        {
            get
            {
                return System.IO.File.Exists(FILE_PATH);
            }
        }

        public string DOWNLOAD_PATH
        {
            get
            {
                UploadPathHelper uploadPathHelper = new UploadPathHelper();

                return uploadPathHelper.GetDownloadUrl(RELATIVE_PATH);
            }
        }

        public long SIZE
        {
            get
            {
                return Convert.ToInt64(dataRow["FILE_SIZE"]);
            }
        }

        public string SIZE_FORMATED
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = SIZE;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }

                string result = String.Format("{0:0.##} {1}", len, sizes[order]);

                return result;
            }
        }

        public string HASH
        {
            get
            {
                return Convert.ToString(dataRow["FILEHASH"]);
            }
        }

        public string FILE_LABEL
        {
            get
            {
                return Convert.ToString(dataRow["FILE_LABEL"]);
            }
        }


        public FILE_TYPES FILE_TYPE
        {
            get
            {
                return fileFuncs.GetFileType(ORIGINAL_FILE_NAME);               
            }
        }

        public string PACKAGE_NAME
        {
            get
            {
                return Convert.ToString(dataRow["PACKAGE_NAME"]);
            }
        }

        public bool IS_sAMPLE
        {
            get
            {
                return Convert.ToInt16(dataRow["IS_sAMPLE"]) == 1 ? true : false;
            }
        }



    }
}
