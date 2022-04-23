using System;
using System.Collections.Generic;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Title = "MiniComputer";

            while (true)
            {
                Write($"<{FormatPath(Globals.currentPath)}>: ");
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

        //Finds the referenced command by the user in order to 
        public static void FindCommand(string command, string[] arguments)
        {
            switch (command)
            {
                case "new":
                    //If not enough args, return
                    if (arguments.Length < 2)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    //if args are null, return
                    if (arguments[0] == null || arguments[1] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    if (arguments[0].ToLower() == "dir" || arguments[0].ToLower() == "directory")
                    {
                        CreateDirectory(arguments[1]);
                    }
                    else if (arguments[0].ToLower() == "file")
                    {
                        CreateFile(arguments[1]);
                    }
                    else
                    {
                        Globals.WriteError($"{arguments[0]} not recognised.");
                    }
                    break;

                case "cd":
                    //if args are null, return
                    if (arguments[0] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    ChangeDirectory(arguments[0]);
                    break;

                case "list":
                case "ls":
                    bool writen = false;
                    for (int i = 0; i < Globals.currentPath.Last().directories.Count(); i++)
                    {
                        WriteLine($"   [dir] {Globals.currentPath.Last().directories[i].name}");
                        writen = true;
                    }
                    for (int i = 0; i < Globals.currentPath.Last().files.Count(); i++)
                    {
                        WriteLine($"   ({Globals.currentPath.Last().files[i].extension}) {Globals.currentPath.Last().files[i].name}");
                        writen = true;
                    }
                    if (writen == false) WriteLine("Directory is empty.");
                    break;

                case "del":
                    //if args are null, return
                    if (arguments[0] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    Delete(arguments[0]);
                    break;

                case "clear":
                    Clear();
                    break;

                case "open":
                    //if args are null, return
                    if (arguments[0] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    FileViewer.OpenFile(arguments[0], Globals.currentPath);
                    break;

                case "edit":
                    //if args are null, return
                    if (arguments[0] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    FileEditor.EditFile(arguments[0], Globals.currentPath);
                    break;

                case "rename":
                    //If not enough args, return
                    if (arguments.Length != 2)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    //if args are null, return
                    if (arguments[0] == null || arguments[1] == null)
                    {
                        Globals.WriteError("Please write the necessary arguments.");
                        return;
                    }

                    Rename(arguments[0], arguments[1]);
                    break;
                
                default:
                    Globals.WriteError("No such command exists.");
                    break;
            }
        }

        public static void CreateFile(string fileName)
        {
            if (fileName == null || fileName == "")
            {
                Globals.WriteError("Cannot create file with no name.");
                return;
            }

            int count = fileName.Split('.').Length - 1;
            if (count > 1) { Globals.WriteError("Cannot have multiple file extensions."); return; }

            if (File.FindInChildren(fileName, Globals.currentPath) != null) { Globals.WriteError("Name is already used."); return; }
            if (Directory.FindInChildren(fileName, Globals.currentPath) != null) { Globals.WriteError("Name is already used."); return; }

            File newFile = new File(fileName, Globals.currentPath);
            WriteLine($"Created file {newFile.name}.{newFile.extension}");
        }

        public static void CreateDirectory(string dirName)
        {
            if (dirName == null || dirName == "") { Globals.WriteError("Cannot create directory with no name."); return; }
            
            if (File.FindInChildren(dirName, Globals.currentPath) != null) { Globals.WriteError("Name is already used."); return; }
            if (Directory.FindInChildren(dirName, Globals.currentPath) != null) { Globals.WriteError("Name is already used."); return; }

            Directory newDir = new Directory(dirName, Globals.currentPath);

            ChangeDirectory(dirName);

            WriteLine("Created directory " + newDir.name);
        }

        public static void Rename(string oldName, string newName)
        {
            File? toRename = File.FindInChildren(oldName, Globals.currentPath);
            if (toRename == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }
            string oldExtension = toRename.extension;

            toRename.Rename(newName);
            WriteLine($"Renamed {oldName}.{oldExtension} to {toRename.name}.{toRename.extension}.");
        }

        public static void Delete(string name)
        {
            int idx = name.LastIndexOf('.');
            if (idx != -1)name = name[..idx];

            Item.DeleteItem(name, Globals.currentPath);
        }

        public static void ChangeDirectory(string dirName)
        {
            if (dirName == "<=")
            {
                if (Globals.currentPath.Last() == Globals.rootDirectory)
                {
                    Globals.WriteError("Cannot go back further than Main");
                    return;
                }
                dirName = FormatPath(Globals.currentPath[Globals.currentPath.Length - 1].path);
            }
            Directory[] newPath = UnFormatPath(dirName);
            if (newPath == null) return;

            Globals.currentPath = newPath;
            Clear();
        }

        public static string FormatPath(Directory[] path)
        {
            string output = Globals.rootDirName;
            for (int i = 1; i < path.Length; i++)
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
                Directory? directory = Directory.FindInChildren(splitedPath[0], Globals.currentPath);
                if (directory != null)
                {
                    output = new Directory[directory.path.Count() + 1];

                    for (int i = 0; i < directory.path.Count(); i++)
                    {
                        output[i] = directory.path[i];
                    }

                    Directory? lastDir = Directory.FindInChildren(splitedPath[0], Globals.currentPath);
                    if (lastDir != null)
                    {
                        output[output.Length - 1] = lastDir;
                    }
                }
                else Globals.WriteError("No such directory exists.");


            }
            else
            {
                output = Directory.FindPath(splitedPath);
            }

            return output;
        }
    }
}