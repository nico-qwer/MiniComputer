using System;

namespace MiniComputer
{
    class File : Item
    {
        public string extension = "txt";
        public List<string> content = new List<string>() { "" };

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
                Globals.WriteError("Cannot rename to nothing.");
                return;
            }

            int count = newName.Split('.').Length - 1;
            if (count > 1) { Globals.WriteError("Cannot have multiple file extensions."); return; }

            int idx = newName.LastIndexOf('.');
            if (idx != -1)
            {
                if (newName[(idx + 1)..].Length != 3) Globals.WriteError("File extensions must be 3 characters.");
                else extension = newName[(idx + 1)..];

                name = newName[..idx];
            }
            else
            {
                name = newName;
            }
        }

        public static File? FindInChildren(string _name, Directory[] currentPath)
        {
            File? output = null;

            for (int i = 0; i < currentPath.Last().files.Count(); i++)
            {
                if (currentPath.Last().files[i].name == _name)
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
                Globals.WriteError("Cannot move nowhere.");
                return;
            }
            if (path == null) return;

            path.Last().files.Remove(this);
            path = newPath;
            path.Last().files.Add(this);

        }
    }
}