﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDevelopmentInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            KillProcessThatContainsName("choco");
            var commands = GetCommands();
            foreach (var installCommand in commands)
            {
                Console.Out.WriteLine(installCommand.Description);
                ExecuteCommandSync(installCommand.command);
            }
            Console.WriteLine("Press enter to exit");
            Console.Read();

        }

        private static IEnumerable<InstallCommand> GetCommands()
        {
            return new List<InstallCommand>()
             {
               new InstallCommand()
               {
                 command =
                   @"@powershell -NoProfile -ExecutionPolicy Bypass -Command ""iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin",
                 Description = "installing Choco"
               },

               new InstallCommand()
               {
                 command = "choco install -y git.install",
                 Description = "install git"
               },

               new InstallCommand()
               {
                 command = "choco install -y visualstudio2015community",
                 Description = "installing visual studio 2015"
               },


               new InstallCommand()
               {
                 command = "choco install -y tortoisegit",
                 Description = "installing Tortoise git"
               },

                  new InstallCommand()
               {
                 command = "choco install -y atom",
                 Description = "installing Atom"
               },

               new InstallCommand()
               {
                 command = "dism.exe /online /enable-feature:NetFx3 /quiet /norestart",
                 Description = "Enable older dot net for succesful rake builds"

               },
               new InstallCommand()
               {
                   command = "choco install dropbox -y",
                   Description = "installing dropbox"
               },
               new InstallCommand()
               {
                   command = "choco install mobaxterm -y",
                   Description = "installing mobaxterm"
               },
			   new InstallCommand()
               {
                 command = "choco install -y ruby",
                 Description = "install ruby"
               },
			   new InstallCommand()
               {
                 command = "choco install -y ant",
                 Description = "install ant"
               },
			    new InstallCommand()
               {
                 command = "choco install -y gradle",
                 Description = "install gradle"
               }, new InstallCommand()
               {
                 command = "choco install -y maven",
                 Description = "install maven"
               }



			   


             };
        }


        public class InstallCommand
        {
            public string Description { get; set; }
            public string command { get; set; }
        }

        private static void KillProcessThatContainsName(string processName)
        {
            var processes = Process.GetProcesses().ToList();
            var matchingProcesses = processes.Where(p => p.ProcessName.Contains(processName)).ToList();
            foreach (var process in matchingProcesses)
            {
                try
                {
                    Console.WriteLine(process.ProcessName);
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Win32Exception winException)
                {
                    Console.WriteLine(winException);
                }
                catch (InvalidOperationException invalidException)
                {
                    Console.WriteLine(invalidException);
                }
            }
        }

        public static void ExecuteCommandSync(string command)
        {
            Console.WriteLine("Executing command: " + command);
            try
            {
                var procStartInfo =
                    new ProcessStartInfo("cmd", "/c " + command)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                proc.Start();
                while (!proc.HasExited)
                    Console.WriteLine(proc.StandardOutput.ReadLine());
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Following execution command failed: " + command);
                Console.WriteLine("Reason: " + e.Message);
            }
        }

    }
}