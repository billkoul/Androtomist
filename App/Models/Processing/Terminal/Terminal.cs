﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;

namespace Androtomist.Models.Processing
{
    public class Terminal
    {
        //fill in the VB name of the Android OS to use (ie Android 7.1) and the snapshot of the clean device to restore (ie Snapshot 3)
        private string startVM = "cd C:\\Program Files\\Oracle\\VirtualBox && VBoxManage.exe startvm \"Android 7.1\"";// use --type headless after debug to not show the vm window (faster) 
        private string poweroffVM = "cd C:\\Program Files\\Oracle\\VirtualBox && VBoxManage.exe controlvm \"Android 7.1\" poweroff";
        private string restoreVM = "cd C:\\Program Files\\Oracle\\VirtualBox && VBoxManage.exe snapshot \"Android 7.1\" restore \"Snapshot 3\"";
        public bool initialize;

        public Terminal(bool novm = false)
        {
            initialize = init(novm);
        }

        public bool init(bool novm = false)
        {
            if (novm) return true;
            cmd(startVM);

            return true;
        }

        public List<string> exec(string command = "")
        {
            List<string> resultList = new List<string>();
            using (PowerShell ps = PowerShell.Create())
            {
                var results = ps.AddScript(command).Invoke();

                foreach (var res in results)
                    resultList.Add(res.ToString());
            }
            return resultList;
        }

        public string cmd(string command = "", int timeout = 3000)
        {
            string result = "";
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            proc.ErrorDataReceived += (sender, errorLine) => {  };
            proc.OutputDataReceived += (sender, outputLine) => { if (outputLine.Data != null) result += outputLine.Data; };
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();
            proc.WaitForExit(timeout);

            return result;
        }

        public bool dispose()
        {
            cmd(poweroffVM);
            cmd(restoreVM);

            return true;
        }

    }
}
