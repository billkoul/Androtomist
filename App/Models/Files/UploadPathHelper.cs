using System;
using System.IO;

namespace Androtomist.Models.Files
{

    public class UploadPathHelper
    {
        private string GetWebRootPath()
        {
            return System.Web.HostingEnvironment.Current.WebRootPath;
        }

        public string GetUploadPath(bool create = false, string extra_path = "")
        {
            FileFuncs fileFuncs = new FileFuncs();

            //fileFuncs.FolderClearOldFiles(Path.Combine(GetWebRootPath(), "files", "uploads"));

            var uploadPath = !string.IsNullOrEmpty(extra_path) ? Path.Combine(GetWebRootPath(), "files", "uploads", extra_path) : Path.Combine(GetWebRootPath(), "files", "uploads");

            if (create) fileFuncs.FolderCreate(uploadPath);

            return uploadPath;
        }

        public string GetLogPath(bool create = false, string extra_path = "")
        {
            FileFuncs fileFuncs = new FileFuncs();

            //fileFuncs.FolderClearOldFiles(Path.Combine(GetWebRootPath(), "logs"));

            var logsPath = !string.IsNullOrEmpty(extra_path) ? Path.Combine(GetWebRootPath(), "logs", extra_path) : Path.Combine(GetWebRootPath(), "logs");

            if (create) fileFuncs.FolderCreate(logsPath);

            return logsPath;
        }

        public string GetStorePath()
        {
            return "F\\" + DateTime.Now.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture) + "\\" + DateTime.Now.ToString("MM", System.Globalization.CultureInfo.InvariantCulture) + "\\" + DateTime.Now.ToString("dd", System.Globalization.CultureInfo.InvariantCulture) + "\\";
        }

        public string GetDownloadUrl(string absolute_path)
        {
            return string.Format("{0}", System.Text.RegularExpressions.Regex.Replace(absolute_path, System.Text.RegularExpressions.Regex.Escape(GetWebRootPath()), "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Replace(@"\", "/"));
        }

    }
}
