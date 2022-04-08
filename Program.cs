using System;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static string currentDirectory = "Main/";
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

        public string name;
        public string directory;
        public int size;
        public string type;

        public File(string newName, string newDirectory)
        {
            Rename(newName);
            directory = newDirectory;

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