﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSPModelLessModlets
{
    using System.Diagnostics;
    using System.IO;
    public static class Program
    {
        const string FolderName = "GenericEventHandler";
        public static void Main()
        {
            var ok = true;
            // Check if we have the environment variable setup.
            var destPath = System.Environment.GetEnvironmentVariable("KSPGameData");
            if(string.IsNullOrEmpty(destPath))
            {
                Console.WriteLine("No environment variable KSPGameData found or it is Empty. See readme.txt");
                ok = false;
            }

            if(ok)
            {
                // Check that the directory exists.
                if (!System.IO.Directory.Exists(destPath))
                {
                    Console.WriteLine("The game data folder does not exist, please check your environment variable, see readme.txt");
                    ok = false;
                }

                // Check that we are in the right directory
                var files = Directory.GetFiles(destPath, "ModuleManager.*.dll", SearchOption.TopDirectoryOnly);

                switch (files.Length)
                {
                    case 0:
                        Console.WriteLine("Module Manager not found in GameData directory, please check your environment variables and that you have ModuleManager installed. See Readme.txt");
                        break;
                    case 1:
                        Console.WriteLine("Modulemanager detected.");
                        break;
                    default:
                        Console.WriteLine("Warning! More than one ModuleManager found, delete all the previous versions.");
                        ok = false;
                        break;
                }

                // Check if the GEH Folder is created if not we create one.
                var modletFolder = Path.Combine(destPath, FolderName);
                if (ok && !Directory.Exists(modletFolder))
                {
                    Directory.CreateDirectory(modletFolder);
                }

                if(ok)
                {
                    DoCopy(modletFolder);
                }
            }

            Console.WriteLine("Please press any key.");
            Console.ReadKey();
        }

        private static void DoCopy(string modletFolder)
        {
            // we are in the debug folder so we need to step back from debug, bin and then up to GEH
            const string relativePath = "../../GenericEventHandler";
            if (!Directory.Exists(relativePath))
            {
                Console.WriteLine("relative path didn't work, this only works by running in debug from visual studio. F5");
                return;
            }

            // Now run XCopy to copy the files
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "xcopy",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = "\"" + relativePath + "\" " + "\"" + modletFolder + "\" /x /s /y /z /c"
            };

            try
            {
                using (var p = Process.Start(startInfo))
                {
                    p.WaitForExit();
                }

                Console.WriteLine("All done.");
            } catch (Exception ex)
            {
                Console.WriteLine("Fatal error with xcopy");
                Console.WriteLine(ex);
            }
        }
    }
}