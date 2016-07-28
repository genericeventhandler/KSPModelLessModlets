// <copyright file="Program.cs" company="GenericEventHandler">
//     Copyright (c) GenericEventHandler all rights reserved. Licensed under the Mit license.
// </copyright>

namespace KSPModelLessModlets
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// This application copies the scripts from the working directory, directly into the game
    /// directory when it is run
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// the folder name inside the GameData directory that will hold the modlets
        /// </summary>
        private const string FolderName = "GenericEventHandler";

        /// <summary>
        /// The main entry point for the file copy program
        /// </summary>
        public static void Main()
        {
            var ok = true;

            // Check if we have the environment variable setup.
            var destPath = System.Environment.GetEnvironmentVariable("KSPGameData");
            if (string.IsNullOrEmpty(destPath))
            {
                Console.WriteLine("No environment variable KSPGameData found or it is Empty.\nHave you restarted VS?\n See readme.txt");
                ok = false;
            }

            Console.WriteLine("Found KSP Path :" + destPath);

            if (ok)
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

                if (ok)
                {
                    DoCopy(modletFolder);
                }
            }

            Console.WriteLine("Please press any key.");
            Console.ReadKey();
        }

        /// <summary>
        /// Copy the folder to the destination
        /// </summary>
        /// <param name="modletFolder">the destination folder</param>
        private static void DoCopy(string modletFolder)
        {
            // we are in the debug folder so we need to step back from debug, bin and then up to GEH
            const string relativePath = @"..\..\GenericEventHandler";
            if (!Directory.Exists(relativePath))
            {
                Console.WriteLine("relative path didn't work, this only works by running in debug from visual studio. F5");
                return;
            }

            // Now run XCopy to copy the files ( /z networked restartable, /s directories and non
            // empty subdirectories, /y yes to prompts, /d only newer files, /c continue if errors /f
            // full output /v check file size
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "xcopy",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = "\"" + relativePath + "\" " + "\"" + modletFolder + "\" /z /s /y /d /c /f /v"
            };

            try
            {
                using (var p = Process.Start(startInfo))
                {
                    p.WaitForExit();
                }

                Console.WriteLine("All done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error with xcopy");
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}