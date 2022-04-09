using System;

using static System.Console;

namespace MiniComputer
{
    internal class Program
    {
        static Directory mainDirectory = Directory.mainDirectory;
        static Directory[] currentPath = new Directory[1] { mainDirectory };
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
            }
        }

        public static void FindCommand(string command, string[] arguments)
        {

        }

        public static void CreateFile(string fileName, Directory[] filePath)
        {
            if (fileName == null || filePath == null)
            {
                WriteError("Cannot create file with no name");
                return;
            }

            File newFile = new File(fileName, filePath);
            WriteLine("Created file " + newFile.name);
        }

        public static void CreateDirectory(string dirName, Directory[] dirPath)
        {
            if (dirName == null|| dirName == "" || dirPath == null)
            {
                WriteError("Cannot create directory with no name");
                return;
            }

            File newDir = new File(dirName, dirPath);
            WriteLine("Created directory " + newDir.name);
        }

        public static void WriteError(string text)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            ResetColor();
        }

        public static string FormatDirectory(Directory[] path)
        {
            string output = "";
            foreach (Directory directory in path)
            {
                output = output + "/" + directory.name;
            }
            return output;
        }
        public static Directory[] UnFormatDirectory(string path)
        {
            string[] splitedPath = path.Split("/");

            Directory[] output = Directory.Find(splitedPath);

            return output;
        }
    }
}