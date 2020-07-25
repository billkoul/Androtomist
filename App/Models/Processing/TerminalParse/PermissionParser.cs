using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Text.Json;

namespace Androtomist.Models.Processing
{
    public class PermissionParser
    {
        /// <summary>
        /// Converts the text output from terminal to json with permissions, may need adjustments. XMLParser class is more stable and robust.
        /// </summary>
        public string ParsePermissionJson(string permissionText, string packageName)
        {
            if (permissionText.Length > 1)
            {
                permissionText = permissionText.Replace("uses-permission: name=", "");
                permissionText = permissionText.Replace("''", "\r\n");
                permissionText = permissionText.Replace("'", "\r\n");
                permissionText = permissionText.Replace("permission:", "\r\n");
                permissionText = permissionText.Replace("name=", "");
                permissionText = permissionText.Replace("\r\n", "\",\"");
                permissionText = permissionText.Replace("package: ", "\"package\":\"");
                permissionText = permissionText.Length > 3 ? permissionText.Substring(0, permissionText.Length - 2) : permissionText;
                permissionText = permissionText.Replace("\"\",", "");
            }

            var regex = new Regex(Regex.Escape(","));
            permissionText = regex.Replace(permissionText, ", \"permissions\":[", 1);

            permissionText = "{" + permissionText + "]}";

            return permissionText;
        }

    }
}
