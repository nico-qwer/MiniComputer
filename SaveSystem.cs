using System;
using static System.Console;

namespace MiniComputer
{
    class SaveSystem
    {
        public static string? appPath;
        public static string? filesPath;
        public static string? saveFilePath;

        //Saves a file to the current filesPath
        public static List<string> FormatFile(File file)
        {
            List<string> toWrite = new List<string>(file.content);

            for (int i = 0; i < toWrite.Count(); i++)
            {
                toWrite[i] = "    " + toWrite[i];
            }

            toWrite.Insert(0, "    c======");
            toWrite.Insert(0, "    " + Program.FormatPath(file.path));
            toWrite.Insert(0, "    " + file.name + "." + file.extension);
            toWrite.Insert(0, "    " + file.ID.ToString());
            toWrite.Insert(0, "{");
            toWrite.Add("}");

            return toWrite;
        }

        public static List<string> FormatDirectory(Directory directory)
        {
            List<string> toWrite = new List<string>();
            toWrite.Add("{");

            toWrite.Add("    " + directory.ID.ToString());
            toWrite.Add("    " + directory.name);
            toWrite.Add("    " + Program.FormatPath(directory.path));

            toWrite.Add("}");

            return toWrite;
        }

        public static async void Dump(string saveName)
        {
            if (filesPath == null || saveFilePath == null) return;

            List<string> toWrite = new List<string>();

            toWrite.Add("DIRECTORIES:");
            for (int i = 0; i < Directory.allDirectories.Count(); i++)
            {
                if (Directory.allDirectories[i].name == Globals.rootDirName) continue;
                toWrite.AddRange(FormatDirectory(Directory.allDirectories[i]));
            }

            toWrite.Add("FILES:");
            for (int i = 0; i < File.allFiles.Count(); i++)
            {
                toWrite.AddRange(FormatFile(File.allFiles[i]));
            }

            await System.IO.File.WriteAllLinesAsync(saveFilePath, toWrite);
        }

        public static void Load()
        {
            string[] saveFiles = Directory.GetFiles(filesPath)
            int selectedFile = 0;

            while (hasBeenSelected == false)
            {
                Clear();
                WriteLine("Save file to load:");
            
                for (int i = 0; i < saveFiles.Length; i++;)
                {
                    if (selectedFile == i) Globals.WriteWithColor(saveFiles[i].Split(@"\").Last());
                    else WriteLine(saveFiles[i].Split(@"\").Last());
                }

                ConsoleKeyInfo keyInfo = ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedFile <= 0) break;
                        selectedFile--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedFile >= saveFiles.Length - 1) break;
                        selectedFile++;
                        break;

                    case ConsoleKey.Enter:
                        saveFilePath = saveFiles[i];
                        break;
                }
            }

            if (filesPath == null || saveFilePath == null) return;
            string[] lines = System.IO.File.ReadAllLines(saveFilePath);

            if (lines.Length < 2) return;

            //if (lines[0] != "DIRECTORIES:") { Globals.WriteError($"Save file invalid at line 0:{lines[0]}"); return; }

            string currentlyLoading = "Directories";

            //Item loading
            int recordStart = 0;
            //bool foundDirectory = false;
            string newName = "";
            int newID = 0;
            Directory[] newPath = new Directory[1] { Globals.rootDirectory };

            //File loading
            List<string> newFileContent = new List<string>();

            //Directory loading
            bool isLoadingDirs = true;
            List<File> newFileChildren = new List<File>();
            List<Directory> newDirChildren = new List<Directory>();
            Directory currentDirectory = Globals.rootDirectory;

            WriteLine("loading directories...");
            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i];
                if (currentLine.StartsWith("    ")) { currentLine = currentLine.Remove(0, 4); }

                if (currentlyLoading == "Directories")
                {
                    if (currentLine == "FILES:")
                    {
                        currentlyLoading = "Files";
                        WriteLine("loading files...");
                        continue;
                    }

                    //Detects begenning of directory
                    if (currentLine == "{")
                    {
                        recordStart = i;
                        newName = "";
                        newFileChildren = new List<File>();
                        newDirChildren = new List<Directory>();
                        continue;
                    }
                    //Detects directory ID
                    if (i == recordStart + 1)
                    {
                        if (Int32.TryParse(currentLine, out newID) == false)
                        {
                            Globals.WriteError($"Save file invalid at line {i}: {currentLine}");
                            return;
                        }
                    }
                    //Detects directory name
                    if (i == recordStart + 2)
                    {
                        newName = currentLine;
                    }
                    //Detects directory path
                    if (i == recordStart + 3)
                    {
                        newPath = Program.UnFormatPath(currentLine);
                    }
                    //Detects end of directory creation
                    if (currentLine == "}")
                    {
                        Directory? newDirectory = Program.CreateDirectory(newName, newPath, newID, false);
                        if (newDirectory == null) return;

                        newDirectory.files = newFileChildren;
                        newDirectory.directories = newDirChildren;
                    }
                    //Detects directories
                    if (i >= recordStart + 5 && isLoadingDirs == true)
                    {
                        newFileContent.Add(currentLine);
                        continue;
                    }

                }
                else if (currentlyLoading == "Files")
                {
                    //Detects begenning of file
                    if (currentLine == "{")
                    {
                        recordStart = i;
                        newName = "";
                        newFileContent = new List<string>();
                        continue;
                    }
                    //Detects file ID
                    if (i == recordStart + 1)
                    {
                        if (Int32.TryParse(currentLine, out newID) == false)
                        {
                            Globals.WriteError($"Save file invalid at line {i}: {currentLine}");
                            return;
                        }
                        continue;
                    }
                    //Detects file name and extension
                    if (i == recordStart + 2)
                    {
                        newName = currentLine;
                        continue;
                    }
                    //Detects file path
                    if (i == recordStart + 3)
                    {
                        newPath = Program.UnFormatPath(currentLine);
                        continue;
                    }
                    //Detects delimitation between info and content
                    if (i == recordStart + 4 && currentLine != "c======")
                    {
                        Globals.WriteError($"Save file invalid at line {i}: {currentLine}");
                        return;
                    }
                    //Detects end of file creation
                    if (currentLine == "}")
                    {
                        File? newFile = Program.CreateFile(newName, newPath, newID, false);

                        if (newFile == null) return;

                        newFile.content = newFileContent;
                        continue;
                    }
                    //Detects content
                    if (i >= recordStart + 5)
                    {
                        newFileContent.Add(currentLine);
                        continue;
                    }
                }
                else
                {
                    Globals.WriteError($"Save file invalid at line {i}: {currentLine}");
                    return;
                }
            }

            WriteLine($"Loaded save file <{saveFilePath}>");
        }

        public static void LoadPath()
        {
            appPath = System.IO.Directory.GetCurrentDirectory();

            if (appPath == null) { Globals.WriteError($"exe file path not found."); return; }

            filesPath = System.IO.Path.Combine(appPath, "StoredFiles");

            if (System.IO.Directory.Exists(filesPath) == false)
            {
                System.IO.Directory.CreateDirectory(filesPath);
            }
        }
    }
}