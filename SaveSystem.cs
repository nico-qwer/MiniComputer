using System;
using static System.Console;

namespace MiniComputer
{
    class SaveSystem
    {
        public static string? appPath;
        public static string? filesPath;

        //Saves a file to the current filesPath
        public static List<string> FormatFile(File file)
        {
            List<string> toWrite = new List<string>(file.content);
            toWrite.Insert(0, "======");
            toWrite.Insert(0, Program.FormatPath(file.path));
            toWrite.Insert(0, file.name + "." + file.extension);
            toWrite.Insert(0, file.ID.ToString());
            toWrite.Insert(0, "{");
            toWrite.Insert(toWrite.Count(), "}");

            return toWrite;
        }

        public static List<string> FormatDirectory(Directory directory)
        {
            List<string> toWrite = new List<string>();
            toWrite.Insert(0, "======");
            toWrite.Insert(0, Program.FormatPath(directory.path));
            toWrite.Insert(0, directory.name);
            toWrite.Insert(0, directory.ID.ToString());
            toWrite.Insert(0, "{");
            toWrite.Insert(toWrite.Count(), "}");

            return toWrite;
        }

        public static async void Dump()
        {
            if (filesPath == null) return;

            List<string> toWrite = new List<string>();

            toWrite.Add("FILES:");
            for (int i = 0; i < File.allFiles.Count(); i++)
            {
                toWrite.AddRange(FormatFile(File.allFiles[i]));
            }

            toWrite.Add("DIRECTORIES:");
            for (int i = 0; i < Directory.allDirectories.Count(); i++)
            {
                if (Directory.allDirectories[i].name == Globals.rootDirName) continue;
                toWrite.AddRange(FormatDirectory(Directory.allDirectories[i]));
            }

            await System.IO.File.WriteAllLinesAsync(Path.Combine(filesPath, "SaveFile"), toWrite);
        }

        public static void Load()
        {
            if (filesPath == null) return;
            string[] lines = System.IO.File.ReadAllLines(filesPath);
        }

        public static void LoadPath()
        {
            appPath = System.IO.Directory.GetCurrentDirectory();

            if (appPath == null) { Globals.WriteError($"exe file path not found. (What happened?? This is imposible?!)"); return; }

            filesPath = System.IO.Path.Combine(appPath, "StoredFiles");

            if (System.IO.Directory.Exists(filesPath) == false)
            {
                System.IO.Directory.CreateDirectory(filesPath);
            }

            WriteLine($"Loaded path: <{filesPath}>");
        }

        public static void LoadFiles()
        {
            if (appPath == null) { return; }

            string[] lines = System.IO.File.ReadAllLines(appPath);
        }
    }
}