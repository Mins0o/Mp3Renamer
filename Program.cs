using System;
using System.IO;
using Microsoft.Win32;
using TagLib;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type in the full folder path that contains the .mp3 files you want to rename.");
            string userInput = Console.ReadLine();
            FileInfo[] userDirectoryFiles = null;
            try
            {
                var userDirectory = new DirectoryInfo(@userInput);
                userDirectoryFiles = userDirectory.GetFiles("*.mp3");
            }
            catch
            {
                System.Console.WriteLine("Could not find the directory: " + userInput);
                System.Console.ReadLine();
                return;
            }
            if (userDirectoryFiles == null || userDirectoryFiles.Length == 0)
            {
                return;
            }
            renameToArtistTitle(userDirectoryFiles);
            var b = 0;
        }
        private static void renameToArtistTitle(FileInfo[] files)
        {

            foreach (FileInfo file in files)
            {
                string filePath = file.FullName;
                TagLib.File musicFile = TagLib.File.Create(filePath);
                string artist = String.Join("/",musicFile.Tag.Performers);
                if (artist == null)
                {
                    artist = "<unknown>";
                }
                artist = MakeValidFileName(artist);
                string title = musicFile.Tag.Title;
                musicFile.Dispose();
                if (title != null)
                {
                    title = MakeValidFileName(title);
                    try
                    {
                        System.IO.File.Move(filePath, "C:/Users/steng/Desktop/New folder/" + artist + "-" + title + ".mp3");
                    }
                    catch (IOException e)
                    {
                        if (e.Message == "Cannot create a file when that file already exists.\r\n")
                        {
                            System.IO.File.Move(filePath, "C:/Users/steng/Desktop/New folder/" + artist + "-" + title + "_.mp3");
                        }
                    }
                    
                }
            }
        }
        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
    }
}
