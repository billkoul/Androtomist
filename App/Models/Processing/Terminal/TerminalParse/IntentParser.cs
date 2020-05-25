using System;
using System.Data;
using System.Collections.Generic;
using Androtomist.Models.Database;
using Androtomist.Models.Database.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Androtomist.Models.Processing
{
    public class IntentParser
    {
        /// <summary>
        /// Converts the text output from terminal to json with permissions, may need adjustments. I din't have time to write an xml parsing function.
        /// TODO: Change this silly function with a function that parses XML and reads the names of the intents
        /// </summary>
        public string ParseIntentJson(string intentOutput)
        {

            int ifIndex = intentOutput.IndexOf("intent-filter");
            intentOutput = intentOutput.Substring(ifIndex);
            intentOutput = intentOutput.Replace("\"", "");
            intentOutput = intentOutput.Replace("(", "[");
            intentOutput = intentOutput.Replace(")", "]");

            MatchCollection matches = Regex.Matches(intentOutput, @"(\[(?:\[??[^\[]*?\]))");

            string json = "";
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (capture.ToString().Contains("Raw"))
                        json += capture.ToString();
                }
            }

            json = json.Replace("][", "],[");
            json = json.Replace(":", ":\"");
            json = json.Replace("]", "\"");
            json = json.Replace("[", "");
            json = json.Replace("Raw:", "");
            json = json.Replace(" ", "");
            json = "{\"intent\":[" + json + "]}";

            return json;
        }
    }
}
