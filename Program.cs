using System;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static Directory[] currentPath = new Directory[1] { Globals.mainDirectory };
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
                    CreateDirectory(arguments[1]);
                } 
                else if (arguments[0].ToLower() == "file")
                {
                    CreateFile(arguments[1]);
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
            else if (command == "list")
            {
                bool writen = false;
                for (int i = 0; i < currentPath.Last().directories.Count(); i++)
                {
                    WriteLine($"   [dir] {currentPath.Last().directories[i].name}");
                    writen = true;
                }
                for (int i = 0; i < currentPath.Last().files.Count(); i++)
                {
                    WriteLine($"   ({currentPath.Last().files[i].type}) {currentPath.Last().files[i].name}");
                    writen = true;
                }
                if (writen == false) WriteLine("Directory is empty.");
            }
            else if (command == "del")
            {
                Delete(arguments[0]);
            }
            else if (command == "clear")
            {
                Clear();
            }
            else WriteError("201: No such command exists.");
        }

        public static void CreateFile(string fileName)
        {
            if (fileName == null || fileName == "")
            {
                WriteError("201: Cannot create file with no name.");
                return;
            }

            if (Directory.FindInChildren(fileName, currentPath) != null) 
            {
                WriteError("401: Name is already used.");
                return;
            }

            File newFile = new File(fileName, currentPath);
            WriteLine("Created file " + newFile.name);
        }

        public static void CreateDirectory(string dirName)
        {
            if (dirName == null|| dirName == "")
            {
                WriteError("201: Cannot create directory with no name.");
                return;
            }

            Directory newDir = new Directory(dirName, currentPath);
            WriteLine("Created directory " + newDir.name);
        }

        public static void Delete(string name)
        {
            Item.DeleteItem(name, currentPath);
        }

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            ResetColor();
        }

        public static string FormatDirectory(Directory[] path)
        {
            string output = Globals.mainDirectory.name;
            WriteLine(path.Length.ToString());
            for(int i = 1; i < path.Length; i++) 
            {   
                output = output + "/" + path[i].name;
            }
            return output;
        }
        public static Directory[] UnFormatDirectory(string path)
        {
            string[] splitedPath = path.Split("/"); 
            Directory[] output = default!;

            if (splitedPath.Length == 1 && splitedPath[0] != Globals.mainDirectory.name)
            {
                Directory? directory = Directory.FindInChildren(splitedPath[0], currentPath);
                if (directory != null) 
                {
                    output = new Directory[directory.path.Count() + 1];
                    Directory? lastDir = Directory.FindInChildren(splitedPath[0], currentPath);
                    if (lastDir != null)
                    {
                        output[output.Length - 1] = lastDir;
                    }
                }

            } else
            {
                output = Directory.FindPath(splitedPath);
            }

            return output;
        }
    }
}