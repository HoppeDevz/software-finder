using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExePath
{
    internal class Program
    {

        static void Main(string[] args)
        {

            /*

            SoftwareFinder finder = new SoftwareFinder();
            string processName = finder.GetSoftwareProcessNameByFriendlyName("dbeaver");

            Console.WriteLine(processName);
            Console.ReadLine();

            return;

            */

            if (args.Length == 0) return;

            SoftwareFinder finder = new SoftwareFinder();

            switch (args[0])
            {

                case "Get-All-Installed-Softwares":
                {

                    List<string> softwares = finder.GetAllProgramsInstalled();

                    foreach (string software in softwares) { 
                        
                        Console.WriteLine(software);
                    }


                    return;
                }

                case "Get-Process-Name-By-Friendly-Name":
                {
                    if (args.Length < 2) return;

                    string processName = finder.GetSoftwareProcessNameByFriendlyName(args[1]);
                    Console.Write(processName);

                    return;
                }

                default: return;
            }

        }
    }
}
