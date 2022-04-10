using System;
using System.Collections.Generic;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static Directory[] currentPath = new Directory[1] { Globals.rootDirectory };
        static File? openFile;

        static void Main(string[] args)
        {
            Title = "MiniComputer";
            
            while(true)
            {
                Write($"<{FormatPath(currentPath)}>: ");
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
                if (arguments.Length != 2)
                {
                    WriteError("Please write the necessary arguments.");
                    return;
                }

                if (arguments[0] == null || arguments[1] == null)
                {
                    WriteError("Please write the necessary arguments.");
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
                    WriteError($"{arguments[0]} not recognised.");
                }
            }
            else if (command == "cd")
            {
                if (arguments[0] == null)
                {
                    WriteError("Please write the necessary arguments.");
                    return;
                }

                ChangeDirectory(arguments[0]);
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
            else if (command == "open")
            {
                if (arguments[0] == null)
                {
                    WriteError("Please write the necessary arguments.");
                    return;
                }

                OpenFile(arguments[0]);
                
            }
            else WriteError("No such command exists.");
        }

        public static void OpenFile(string name)
        {
            openFile = File.FindInChildren(name, currentPath);

            if (openFile == null)
            {
                WriteError("No such file exists.");
                return;
            }

            Clear();

            bool writen = false;
            for (int i = 0; i < openFile.content.Count(); i++)
            {
                writen = true;
                Write(i + "| ");
                WriteLine(openFile.content[i]);
            }

            if (writen == false)
            {
                WriteLine("File is empty.");
            }

            var keyInfo = ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Q)
            {
                keyInfo = ReadKey(true);
            }
            Clear();
        }

        public static void CreateFile(string fileName)
        {
            if (fileName == null || fileName == "")
            {
                WriteError("Cannot create file with no name.");
                return;
            }

            if (File.FindInChildren(fileName, currentPath) != null) 
            {
                WriteError("Name is already used.");
                return;
            }

            File newFile = new File(fileName, currentPath);
            WriteLine("Created file " + newFile.name);
        }

        public static void CreateDirectory(string dirName)
        {
            if (dirName == null|| dirName == "")
            {
                WriteError("Cannot create directory with no name.");
                return;
            }
            if (Directory.FindInChildren(dirName, currentPath) != null) 
            {
                WriteError("Name is already used.");
                return;
            }

            Directory newDir = new Directory(dirName, currentPath);

            ChangeDirectory(dirName);

            WriteLine("Created directory " + newDir.name);
        }

        public static void Delete(string name)
        {
            Item.DeleteItem(name, currentPath);
        }
        
        public static void ChangeDirectory(string dirName)
        {
            if (dirName == "<=")
            {
                if (currentPath.Last() == Globals.rootDirectory)
                {
                    WriteError("Cannot go back further than Main");
                    return;
                }
                dirName = FormatPath(currentPath[currentPath.Length - 1].path);
            }
            Directory[] newPath = UnFormatPath(dirName);
            if (newPath == null) return;

            currentPath = newPath;
            Clear();
        }

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            Write("Error: ");
            WriteLine(text);
            ResetColor();
        }

        public static string FormatPath(Directory[] path)
        {
            string output = Globals.rootDirName;
            for(int i = 1; i < path.Length; i++) 
            {   
                output = output + "/" + path[i].name;
            }
            return output;
        }
        public static Directory[] UnFormatPath(string path)
        {
            string[] splitedPath = path.Split("/"); 
            Directory[] output = default!;

            if (splitedPath.Length == 1 && splitedPath[0] != Globals.rootDirName)
            {
                Directory? directory = Directory.FindInChildren(splitedPath[0], currentPath);
                if (directory != null) 
                {
                    output = new Directory[directory.path.Count() + 1];

                    for(int i = 0; i < directory.path.Count(); i++) 
                    {
                        output[i] = directory.path[i];
                    }

                    Directory? lastDir = Directory.FindInChildren(splitedPath[0], currentPath);
                    if (lastDir != null)
                    {
                        output[output.Length - 1] = lastDir;
                    }
                } 
                else WriteError("No such directory exists.");


            } else
            {
                output = Directory.FindPath(splitedPath);
            }

            return output;
        }
    }
}