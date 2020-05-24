using System;
using System.IO;
using System.IO.Compression;

namespace Androtomist.Models
{
    public class FileFuncs
    {

        public string FileMD5(string file_path)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = default(System.Security.Cryptography.MD5CryptoServiceProvider);
            FileStream file_stream = default(FileStream);
            byte[] hash = null;
            System.Text.StringBuilder buff = default(System.Text.StringBuilder);
            byte hashByte = 0;

            try
            {

                if (File.Exists(file_path))
                {
                    file_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);

                    md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    md5.ComputeHash(file_stream);

                    hash = md5.Hash;

                    md5.Dispose();
                    file_stream.Close();

                    buff = new System.Text.StringBuilder();
                    foreach (byte hashByte_loopVariable in hash)
                    {
                        hashByte = hashByte_loopVariable;
                        buff.Append(string.Format("{0:X2}", hashByte));
                    }

                    return buff.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        public bool FileUnzip(string zip_file_path, string unzip_path)
        {
            try
            {
                ZipFile.ExtractToDirectory(zip_file_path, unzip_path, true);

                return true;
            }

            catch
            {
                throw new Exception("Cannot unzip file [" + zip_file_path + "].");
            }

        }

        public bool FilesZip(string[] file_paths, string zip_file_path)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(zip_file_path, ZipArchiveMode.Create))
                    foreach (string file_path in file_paths)
                        archive.CreateEntryFromFile(file_path, Path.GetFileName(file_path), CompressionLevel.Fastest);

                return true;
            }
            catch
            {
                throw new Exception("Cannot zip file [" + zip_file_path + "].");
            }

        }


        public bool FileCopy(string file_path, string target_file, bool overwrite)
        {
            try
            {
                File.Copy(file_path, target_file, overwrite);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool FileMove(string file_path, string target_file)
        {
            try
            {
                File.Move(file_path, target_file);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool FileDelete(string file_path)
        {
            int i = 0;

            try
            {
                if (File.Exists(file_path))
                {
                    File.Delete(file_path);
                    while (i < 100 && File.Exists(file_path))
                    {
                        System.Threading.Thread.Sleep(10);
                        i++;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool FolderEmpty(string folder_path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folder_path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    FolderDelete(dir.FullName);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void FolderDelete(string folder_path)
        {
            if (!Directory.Exists(folder_path)) return;

            string[] files = Directory.GetFiles(folder_path);
            string[] dirs = Directory.GetDirectories(folder_path);


            foreach (string file in files)
                FileDelete(file);

            foreach (string dir in dirs)
                FolderDelete(dir);

            int i = 0;

            if (Directory.Exists(folder_path))
            {
                Directory.Delete(folder_path, true);
                while (i < 100 && Directory.Exists(folder_path))
                {
                    System.Threading.Thread.Sleep(10);
                    i++;
                }
            }

        }

        public void FolderCreate(string folder_path)
        {
            int i = 0;

            if (!Directory.Exists(folder_path))
            {
                Directory.CreateDirectory(folder_path);
                while (i < 100 && !Directory.Exists(folder_path))
                {
                    System.Threading.Thread.Sleep(10);
                    i++;
                }
            }
        }

        public void FolderClearOldFiles(string folder_path)
        {
            string[] allfiles = Directory.GetFiles(folder_path, "*.*", SearchOption.AllDirectories);

            foreach (var file_path in allfiles)
            {
                FileInfo fi = new FileInfo(file_path);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-3))
                    FileDelete(file_path);
            }

            FolderRemoveEmptySubs(folder_path);
        }

        public void FolderRemoveEmptySubs(string folder_path)
        {
            //return;
            try
            {
                foreach (var directory in Directory.GetDirectories(folder_path))
                {
                    FolderRemoveEmptySubs(directory);
                    if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                        FolderDelete(directory);
                }
            }
            catch
            {
                // Nothing
            }
        }



        public bool CheckDirectoryAccess(string folder_path)
        {
            bool success = false;
            string TEMP_FILE = "\\tempFile.tmp";
            string fullPath = folder_path + TEMP_FILE;

            if (Directory.Exists(folder_path))
            {
                try
                {
                    FileDelete(fullPath);
                    using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.WriteByte(0xff);
                    }

                    success = File.Exists(fullPath);
                    FileDelete(fullPath);
                }
                catch (Exception)
                {
                    success = false;
                }
            }

            return success;
        }


        public FILE_TYPES GetFileType(string file_path)
        {
            switch (Path.GetExtension(file_path).ToLower().Trim())
            {
                case ".xls": return FILE_TYPES.xls;
                case ".xlsx": return FILE_TYPES.xlsx;
                case ".xlsb": return FILE_TYPES.xlsb;
                case ".xlsm": return FILE_TYPES.xlsm;
                case ".mdb": return FILE_TYPES.mdb;
                case ".accdb": return FILE_TYPES.accdb;
                case ".csv": return FILE_TYPES.csv;
                case ".txt": return FILE_TYPES.txt;
                case ".zip": return FILE_TYPES.zip;
				case ".jpg": return FILE_TYPES.jpg;
				case ".png": return FILE_TYPES.jpg;
				default: return FILE_TYPES.other;
            }
        }
    }
}
