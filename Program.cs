using System;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static Directory mainDirectory = Directory.mainDirectory;
        static Directory[] currentPath = new Directory[1] { mainDirectory };
        //static File? openFile;

        static void Main(string[] args)
        {
            Title = "MiniComputer";
            
            while(true)
            {
                Write($"<{FormatDirectory(currentPath)}>: ");
                string? input = ReadLine();
                if (input == null || input == "") continue;

                string command = input.Split(" ")[0];
                string[] arguments = input.Split(" ").Skip(1).ToArray();
                if (arguments == null) 
                {
                    arguments = new string[0];
                }
                FindCommand(command, arguments);
            }
        }

        public static void FindCommand(string command, string[] arguments)
        {
            if (command == "new")
            {
                if (arguments[0] == null || arguments[1] == null)
                {
                    WriteError("101: Please write the necessary arguments.");
                    return;
                }

                if (arguments[0].ToLower() == "dir" || arguments[0].ToLower() == "directory" )
                {
                    CreateDirectory(arguments[1], currentPath);
                } 
                else if (arguments[0].ToLower() == "file")
                {
                    CreateFile(arguments[1], currentPath);
                }
                else
                {
                    WriteError($"102: {arguments[0]} not recognised.");
                }
            }
            else if (command == "cd")
            {
                if (arguments[0] == null)
                {
                    WriteError("101: Please write the necessary arguments.");
                    return;
                }

                Directory[] newPath = UnFormatDirectory(arguments[0]);
                if (newPath == null) return;

                currentPath = newPath;
                Clear();
            }
            else if (command = "ls")
            {
                for (int i = 0; i > currentPath.Last().directories.Count())
                {
                    WriteLine($"[dir] {currentPath.Last().directories[i].name}");
                }
                for (int i = 0; i > currentPath.Last().files.Count())
                {
                    WriteLine($"({currentPath.Last.files[i].type}) {currentPath.Last().files[i].name}");
                }
            }
            else WriteError("201: No such command exists.");
        }

        public static void CreateFile(string fileName, Directory[] filePath)
        {
            if (fileName == null || filePath == null)
            {
                WriteError("201: Cannot create file with no name.");
                return;
            }

            File newFile = new File(fileName, filePath);
            WriteLine("Created file " + newFile.name);
        }

        public static void CreateDirectory(string dirName, Directory[] dirPath)
        {
            if (dirName == null|| dirName == "" || dirPath == null)
            {
                WriteError("201: Cannot create directory with no name.");
                return;
            }

            if (SearchChildren(dirName, currentPath) == true)
            {
                WriteError($"203: Directory with name {dirName} already exists.")
            }
            
            Directory newDir = new Directory(dirName, dirPath);
            WriteLine("Created directory " + newDir.name);
        }

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            ResetColor();
        }

        public static string FormatDirectory(Directory[] path)
        {
            string output = mainDirectory.name;
            for(int i = 1; i < path.Length; i++) 
            {
                output = output + "/" + path[i].name;
            }
            return output;
        }
        public static Directory[] UnFormatDirectory(string path)
        {
            string[] splitedPath = path.Split("/"); 
            Directory[] output;

            if (splitedPath.Length == 1 && splitedPath[0] != mainDirectory.name)
            {
                output = Directory.FindChildren(splitedPath[0], currentPath);
            } else
            {
                output = Directory.Find(splitedPath);
            }

            return output;
        }
    }
}