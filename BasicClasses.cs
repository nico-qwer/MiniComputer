using System;

using static System.Console;

namespace MiniComputer
{
    class File
    {
        public string name = "Unnamed";
        public Directory[] path;
        public int size = 0;
        public string type = "txt";

        public File(string newName, Directory[] newPath)
        {
            Rename(newName);
            path = newPath;
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ")
            {
                Program.WriteError("201: Cannot rename to nothing.");
                return;
            }

            string newExtension = newName.Split('.').Last();

            if (newExtension != null && newExtension != "")
            {
                name = newName;
                type = newExtension;
            }
            else
            {
                name = newName + "." + type;
            }
        }

        public void Move(Directory[] newPath)
        {
            if (newPath == null)
            {
                Program.WriteError("201: Cannot move nowhere.");
                return;
            }

            path = newPath;

        }
    }

    class Directory
    {
        private static string rootDirName = "Main";
        public static Directory mainDirectory = new Directory(rootDirName, new Directory[0]);
        public string name = "Unnamed";
        public Directory[]? path;
        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();

        public Directory(string newName, Directory[] newPath)
        {
            Rename(newName);
            path = newPath;
            if (newName == rootDirName) return;
            newPath.Last().directories.Add(this);
        }

        public void DeleteDirectory(string _name, Directory[] _path)
        {
            for (int i = 0; i < _path.Last().directories.Count(); i++)
            {
                if (_path.Last().directories[i].name != _name) continue;
                _path.Last().directories.RemoveAt(i);
                return;
            }
            Program.WriteError("404: No such directory exists.");
        }

        public static Directory[] Find(string[] _path)
        {
            Directory[] output = new Directory[_path.Length];
            output[0] = mainDirectory;

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
                    Program.WriteError("404: No such directory exists.");
                }
            }

            return output;
        }

        public static Directory[] FindChildren(string _name, Directory[] currentPath)
        {
            Directory[] output = new Directory[currentPath.Length + 1];
            for(int i = 0; i < currentPath.Length; i++) 
            {
                output[i] = currentPath[i];
            }

            for(int i = 0; i < currentPath.Last().directories.Count(); i++) 
            {
                if (currentPath.Last().directories[i].name == _name)
                {
                    output[output.Length - 1] = currentPath.Last().directories[i];
                    break;
                }
            }
            return output;
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ")
            {
                Program.WriteError("201: Cannot rename to nothing.");
                return;
            }

            name = newName;
        }
    }
}