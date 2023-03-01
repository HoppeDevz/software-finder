using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ExePath
{
    internal class SoftwareFinder
    {

        // EXAMPLE: GetAllProgramsInstalled() -> List('Google Chrome', 'Battle.net', 'Steam')
        public List<string> GetAllProgramsInstalled()
        {

            List<string> programs = new List<string>();

            string path64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string path32 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(path64))
            {

                foreach (string subkey_name in key.GetSubKeyNames())
                {

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {

                        if (subkey.GetValue("DisplayName") != null)
                        {

                            programs.Add(subkey_name);
                        }
                    }
                }
            }

            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(path32))
            {

                foreach (string subkey_name in key.GetSubKeyNames())
                {

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {

                        if (subkey.GetValue("DisplayName") != null)
                        {

                            programs.Add(subkey_name);
                        }
                    }
                }
            }

            return programs;
        }

        // EXAMPLE: GetSoftwareProcessNameByFriendlyName('Google Chrome') -> chrome.exe
        public string GetSoftwareProcessNameByFriendlyName(string softwareName)
        {

            foreach (Process process in Process.GetProcesses())
            {

                try
                {

                    if (
                        process.MainModule != null &&
                        process.MainModule.FileVersionInfo != null &&
                        process.MainModule.FileVersionInfo.FileDescription != null
                    ) {

                        if (process.MainModule.FileVersionInfo.FileDescription.Equals(softwareName, StringComparison.OrdinalIgnoreCase))
                        {

                            return process.ProcessName;
                        }
                        
                        if (process.MainModule.FileVersionInfo.ProductName.ToLower().Contains(softwareName.ToLower()))
                        {
                            return process.ProcessName;
                        }
                    }

                    if (process.ProcessName.ToLower().Contains(softwareName.ToLower()) || process.MainModule.ModuleName.ToLower().Contains(softwareName.ToLower()))
                    {

                        return process.ProcessName;
                    }

                } catch (Exception exception)
                {

                    // DO NOTHING
                }
                
            }

            return null;
        }

        public string GetSoftwareProcessNameByFriendlyNamePS(string softwareName)
        {

            string processName = null;

            string psScript = $@"
                                
                $friendlyName = '{softwareName}'

                $processes = Get-Process

                foreach ($process in $processes) {{

                    if ($process.MainModule.FileVersionInfo.FileDescription -eq $friendlyName) {{

                        Write-Output $process.ProcessName
                        return
                    }}
                }}

                Write-Output ''
            ";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-Command \"{psScript}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                processName = output;
            }

            switch (processName)
            {

                case "": return null;

                default: return processName;
            }
        }

    }
}
