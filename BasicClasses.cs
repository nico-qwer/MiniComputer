using System;

using static System.Console;

namespace MiniComputer
{
    class Globals
    {
        public static string rootDirName = "Main";
        public static Directory rootDirectory = new Directory(rootDirName, new Directory[0]);
    }

    class Item
    {
        public string name = "Unnamed";
        public Directory[] path = default!;

        public static void DeleteItem(string _name, Directory[] _path)
        {
            for (int i = 0; i < _path.Last().directories.Count(); i++)
            {
                if (_path.Last().directories[i].name != _name) continue;
                _path.Last().directories.RemoveAt(i);
                WriteLine($"Successfuly deleted {_name} and it's contents from {_path.Last().name}.");
                return;
            }
            for (int i = 0; i < _path.Last().files.Count(); i++)
            {
                if (_path.Last().files[i].name != _name) continue;
                _path.Last().files.RemoveAt(i);
                WriteLine($"Successfuly deleted {_name} from {_path.Last().name}.");
                return;
            }
            Program.WriteError("No such file or directory exists.");
        }
    }

    class File : Item
    {
        public string type = "txt";
        public List<string> content = new List<string>();

        public File(string newName, Directory[] newPath)
        {
            Rename(newName);
            path = newPath;
            path.Last().files.Add(this);
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ")
            {
                Program.WriteError("Cannot rename to nothing.");
                return;
            }
            string[] splitedName = newName.Split('.');
            string newExtension = splitedName.Last();

            if (newExtension != null && newExtension != "" && splitedName.Length > 1)
            {
                name = newName;
                type = newExtension;
            }
            else
            {
                name = newName + "." + type;
            }
        }

        public static File? FindInChildren(string _name, Directory[] currentPath)
        {
            File? output = null;

            for(int i = 0; i < currentPath.Last().files.Count(); i++) 
            {
                string cutname = currentPath.Last().files[i].name.Substring(0, currentPath.Last().files[i].name.LastIndexOf("."));
                if (currentPath.Last().files[i].name == _name)
                {
                    output = currentPath.Last().files[i];
                    break;
                }
                else if (cutname == _name)
                {
                    output = currentPath.Last().files[i];
                    break;
                }
            }
            return output;
        }

        public void Move(Directory[] newPath)
        {
            if (newPath == null)
            {
                Program.WriteError("Cannot move nowhere.");
                return;
            }
            if (path == null) return;

            path.Last().files.Remove(this);
            path = newPath;
            path.Last().files.Add(this);

        }
    }

    class Directory : Item
    {
        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();

        public Directory(string newName, Directory[] newPath)
        {
            Rename(newName);
            path = newPath;

            if (newName == Globals.rootDirName) return;
            newPath.Last().directories.Add(this);
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
                    Program.WriteError("No such directory exists.");
                }
            }

            return output;
        }

        public static Directory? FindInChildren(string _name, Directory[] currentPath)
        {
            Directory? output = null;

            for(int i = 0; i < currentPath.Last().directories.Count(); i++) 
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
                Program.WriteError("Cannot rename to nothing.");
                return;
            }

            name = newName;
        }
    }
}