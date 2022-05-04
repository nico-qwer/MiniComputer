using System;
using static System.Console;

namespace MiniComputer
{
    class Directory : Item
    {
        public static int currentID = 0;
        public static List<Directory> allDirectories = new List<Directory>();
        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();
        public int ID;

        public Directory(string newName, Directory[] newPath, int newID)
        {
            Rename(newName);
            path = newPath;
            ID = newID;
            currentID++;

            if (newName == Globals.rootDirName) return;
            newPath.Last().directories.Add(this);
            allDirectories.Add(this);
        }

        public static Directory[] FindPath(string[] _path)
        {
            Directory[] output = new Directory[_path.Length];
            output[0] = Globals.rootDirectory;

            for (int i = 1; i < _path.Length; i++)
            {
                if (output[i - 1].directories.Count == 0) break;
                for (int j = 0; j < output[i - 1].directories.Count; j++)
                {
                    if (output[i - 1].directories[j].name == _path[i])
                    {
                        output[i] = output[i - 1].directories[j];
                        break;
                    }
                    Globals.WriteError("No such directory exists.");
                }
            }

            return output;
        }

        public static Directory? FindInChildren(string _name, Directory[] currentPath)
        {
            Directory? output = null;

            for (int i = 0; i < currentPath.Last().directories.Count(); i++)
            {
                if (currentPath.Last().directories[i].name == _name)
                {
                    output = currentPath.Last().directories[i];
                    break;
                }
            }
            return output;
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ")
            {
                Globals.WriteError("Cannot rename to nothing.");
                return;
            }

            name = newName;
        }
    }
}