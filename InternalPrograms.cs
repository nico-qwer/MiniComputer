using System;

using static System.Console;

namespace MiniComputer
{
    class FileViewer
    {
        public static void OpenFile(string name, Directory[] filePath)
        {
            Program.openFile = File.FindInChildren(name, filePath);

            if (Program.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }

            Clear();
            Globals.WriteWithColor("FILE VIEWER V0.1.0", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");

            bool writen = false;
            for (int i = 0; i < Program.openFile.content.Count(); i++)
            {
                writen = true;
                Write(i + "| ");
                WriteLine(Program.openFile.content[i]);
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
    }
    
    class FileEditor
    {
        public static int mode = 0;

        public static void EditFile(string name, Directory[] filePath)
        {
            Program.openFile = File.FindInChildren(name, filePath);

            if (Program.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }

            int selectedLine = 0;

            Clear();
            Globals.WriteWithColor("FILE EDITOR V0.1.0", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");

            bool writen = false;
            for (int i = 0; i < Program.openFile.content.Count(); i++)
            {
                if (i == selectedLine)
                {
                    WriteSelected(i + "| " + Program.openFile.content[i]);
                    continue;
                }

                writen = true;
                Write(i + "| " + Program.openFile.content[i]);
            }

            if (writen == false)
            {
                WriteLine("File is empty.");
            }

        }

        static void WriteSelected(string text)
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            WriteLine(text);
            ResetColor();
        }
    }
}