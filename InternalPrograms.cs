using System;

using static System.Console;

namespace MiniComputer
{
    class FileViewer
    {
        public static void OpenFile(string name, Directory[] filePath)
        {
            Globals.openFile = File.FindInChildren(name, filePath);

            if (Globals.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }

            Clear();
            Globals.WriteWithColor("FILE VIEWER V0.1.0", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");

            bool writen = false;
            for (int i = 0; i < Globals.openFile.content.Count(); i++)
            {
                string number = i.ToString();
                int digits = (int)Math.Floor(Math.Log10(Globals.openFile.content.Count()) + 1);

                for (int j = 0; j < digits - number.Length; j++)
                {
                    number += " ";
                }
                number += "|";

                writen = true;
                Write(number);
                WriteLine(Globals.openFile.content[i]);
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
            Globals.openFile = null;
            Clear();
        }
    }

    class FileEditor
    {
        public static int mode = 0;
        public static int selectedLine = 0;

        public static void EditFile(string name, Directory[] filePath)
        {
            Globals.openFile = File.FindInChildren(name, filePath);
            if (Globals.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }

            bool exit = false;
            while (exit == false)
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedLine <= 0) break;
                        selectedLine -= 1;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedLine >= Globals.openFile.content.Count()) break;
                        selectedLine += 1;
                        break;
                        
                    case ConsoleKey.Q:
                        exit = true;
                        break;
                        
                    default:
                        break;
                }

                Refresh();
            }

            Globals.openFile = null;
            Clear();
        }

        static void WriteSelected(string text)
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            WriteLine(text);
            ResetColor();
        }

        static void Refresh()
        {
            if (Globals.openFile == null) return;

            Clear();
            Globals.WriteWithColor("FILE EDITOR V0.1.0", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");

            bool writen = false;
            for (int i = 0; i < Globals.openFile.content.Count(); i++)
            {
                string number = i.ToString();
                int digits = (int)Math.Floor(Math.Log10(Globals.openFile.content.Count()) + 1);

                for (int j = 0; j < digits - number.Length; j++)
                {
                    number += " ";
                }
                number += "| ";

                writen = true;
                Write(number);
                WriteLine(Globals.openFile.content[i]);
            }

            if (writen == false)
            {
                Write("0 | ");
            }
        }
    }
}