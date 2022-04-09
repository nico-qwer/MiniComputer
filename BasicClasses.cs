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
                Program.WriteError("Cannot rename to nothing");
                return;
            }

            string newExtension = newName.Split('.').Last();

            if (newExtension != null)
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
                Program.WriteError("Cannot move nowhere");
                return;
            }

            path = newPath;

        }
    }

    class Directory
    {
        public static Directory mainDirectory = new Directory("Main", new Directory[0]);
        public string name = "Unnamed";
        public Directory[]? path;
        public List<Directory> directories = new List<Directory>();
        public List<File> files = new List<File>();

        public Directory(string newName, Directory[] newPath)
        {
            Rename(newName);
            path = newPath;
        }

        public static Directory[] Find(string[] path)
        {
            Directory[] output = new Directory[path.Length];
            output[0] = mainDirectory;

            for (int i = 1; i < path.Length; i++)
            {
                for (int j = 0; j < output[i - 1].directories.Count; j++)
                {
                    if (output[i - 1].directories[j].name == path[i])
                    {
                        output[i] = output[i - 1].directories[j];
                        break;
                    }
                }
            }

            return output;
        }

        public void Rename(string newName)
        {
            if (newName == null || newName == " ")
            {
                Program.WriteError("Cannot rename to nothing");
                return;
            }

            name = newName;
        }
    }
}