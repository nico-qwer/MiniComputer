using System;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static string currentDirectory = "Home/";
        static File? openFile;

        static void Main(string[] args)
        {
            Title = "MiniComputer";

            WriteLine("Booting...");
            WriteLine("Booted!");

            Write(currentDirectory + " : ");
            ReadLine();

            ReadLine();
        }

        public static void CreateFile(string fileName, string fileDirectory)
        {
            if (fileName == null || fileDirectory == null) 
            {
                WriteError("Cannot create file with no name"); 
                return;
            }

            File newFile = new File(fileName, fileDirectory);
            WriteLine("Created file " + newFile.name);
        }

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            ResetColor();
        }
    }

    class File 
    {
        public static List<File> files = new List<File>();

        public string name = "Unnamed";
        public string directory = "Home/";
        public int size = 0;
        public string type = "txt";

        public File(string newName, string newDirectory)
        {
            Rename(newName);
            directory = newDirectory;
            files.Add(this);
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
            } else
            {
                name = newName + "." + type;
            }
        }
    }
}