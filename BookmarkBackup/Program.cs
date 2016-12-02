﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace BookmarkBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Hackary to download files to current location since you can't
                // set a variable for a default value on the flag
                if (options.Drive == "Current location")
                {
                    options.Drive = Directory.GetCurrentDirectory();
                }
                else
                {
                    options.Drive = "";
                }

                string chromeSource = (@"C:\Users\%user%\AppData\Local\Google\Chrome\User Data\").Replace("%user%", options.User);
                string chromeDestination = (@"%drive%\Bookmarks\User Data").Replace("%user%", options.User);
                chromeDestination = chromeDestination.Replace("%drive%", options.Drive);

                string firefoxSource = (@"C:\Users\%user%\AppData\Roaming\Mozilla\Firefox\Profiles\").Replace("%user%", options.User);
                string firefoxDestination = (@"%drive%\Bookmarks\Profiles").Replace("%user%", options.User);
                firefoxDestination = firefoxDestination.Replace("%drive%", options.Drive);



                // If -c or -f is used, make sure you're not copying from both chrome and firefox
                if (options.Chrome == true || options.FireFox == true)
                {
                    options.All = false;
                }

                // Copy data from both browsers
                if (options.All == true)
                {
                    // Copying the files
                    if (options.Restore == false)
                    {
                        try
                        {
                            Copy(chromeSource, chromeDestination);
                            Copy(firefoxSource, firefoxDestination);
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                        //Console.WriteLine(chromeSource);
                        //Console.Write(chromeDestination);
                    }

                    // Restoring the files
                    else if (options.Restore == true)
                    {
                        try
                        {
                            Copy(chromeDestination, chromeSource);
                            Copy(firefoxDestination, firefoxSource);
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                    }

                    // Else user only wants to copy from one browser
                    else
                    {
                        // Copying the files
                        if (options.Restore == false)
                        {
                            if (options.Chrome == true)
                            {
                                try
                                {
                                    Copy(chromeSource, chromeDestination);
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine(ex.Message);
                                    Console.ReadLine();
                                }
                                //Console.WriteLine(chromeSource);
                                //Console.Write(chromeDestination);
                            }
                            else if (options.FireFox == true)
                            {
                                try
                                {
                                    Copy(firefoxSource, firefoxDestination);
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine(ex.Message);
                                    Console.ReadLine();
                                }
                                //Console.WriteLine(chromeSource);
                                //Console.Write(chromeDestination);
                            }
                        }

                        // Restoring the files
                        else if (options.Restore == true)
                        {
                            if (options.Chrome == true)
                            {
                                try
                                {
                                    Copy(chromeDestination, chromeSource);
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine(ex.Message);
                                    Console.ReadLine();
                                }
                            }
                            else if (options.FireFox == true)
                            {
                                try
                                {
                                    Copy(firefoxDestination, firefoxSource);
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine(ex.Message);
                                    Console.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            
            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying files to {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
