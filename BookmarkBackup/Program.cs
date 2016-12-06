using System;
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
                        // If you want to copy bookmarks from multiple users on the computer
                        if (options.Multiple == true)
                        {
                            List<string> userList = new List<string>();
                            foreach (string item in Directory.GetDirectories(@"C:\Users"))
                            {
                                userList.Add(item);
                            }
                            var usersFiltered =
                                from string item in userList
                                where !item.Contains("Administrator")
                                where !item.Contains("tech")
                                where !item.Contains("Tech")
                                where !item.Contains("Default")
                                where !item.Contains("Public")
                                where !item.Contains("All Users")
                                select item;

                            Console.WriteLine("Will copy bookmarks from these users: \n");
                            foreach (string item in usersFiltered)
                            {
                                Console.WriteLine(item);
                            }
                            Console.ReadLine();

                            foreach (string item in usersFiltered)
                            {
                                string user;
                                user = item.Split('\\').Last();

                                chromeSource = (@"%user%\AppData\Local\Google\Chrome\User Data\").Replace("%user%", item);
                                chromeDestination = (@"%drive%\Bookmarks\%user%\User Data").Replace("%user%", user);
                                chromeDestination = chromeDestination.Replace("%drive%", options.Drive);

                                firefoxSource = (@"%user%\AppData\Roaming\Mozilla\Firefox\Profiles\").Replace("%user%", item);
                                firefoxDestination = (@"%drive%\Bookmarks\%user%\Profiles").Replace("%user%", user);
                                firefoxDestination = firefoxDestination.Replace("%drive%", options.Drive);
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
                            } //foreach user
                        } // if multiple users
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
                    } // if copying files

                    // Restoring the files
                    else if (options.Restore == true)
                    {
                        if (options.Multiple == true)
                        {
                            List<string> userList = new List<string>();
                            foreach (string item in Directory.GetDirectories(@"C:\Users"))
                            {
                                userList.Add(item);
                            }
                            var usersFiltered =
                                from string item in userList
                                    //where !item.Contains("Administrator")
                                    //where !item.Contains("tech")
                                    //where !item.Contains("Tech")
                                where !item.Contains("Default")
                                where !item.Contains("Public")
                                where !item.Contains("All Users")
                                select item;

                            Console.WriteLine("Will restore bookmarks for these users: \n");
                            foreach (string item in usersFiltered)
                            {
                                Console.WriteLine(item);
                            }
                            Console.ReadLine();

                            foreach (string item in usersFiltered)
                            {
                                string user;
                                user = item.Split('\\').Last();

                                chromeSource = (@"%user%\AppData\Local\Google\Chrome\User Data\").Replace("%user%", item);
                                chromeDestination = (@"%drive%\Bookmarks\%user%\User Data").Replace("%user%", user);
                                chromeDestination = chromeDestination.Replace("%drive%", options.Drive);

                                firefoxSource = (@"%user%\AppData\Roaming\Mozilla\Firefox\Profiles\").Replace("%user%", item);
                                firefoxDestination = (@"%drive%\Bookmarks\%user%\Profiles").Replace("%user%", user);
                                firefoxDestination = firefoxDestination.Replace("%drive%", options.Drive);
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
                            } //foreach user
                        } // if multiple users


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
                } // if copy from all browsers

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
