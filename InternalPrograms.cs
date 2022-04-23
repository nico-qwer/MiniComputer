using System;

using static System.Console;

namespace MiniComputer
{
    class FileViewer
    {
        public static void OpenFile(string name, Directory[] filePath)
        {
            int idx = name.LastIndexOf('.');
            if (idx != -1) name = name[..idx];

            Globals.openFile = File.FindInChildren(name, filePath);

            if (Globals.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            }

            if (Globals.openFile.extension == "img")
            {
                ImageViewer();
                return;
            }

            Clear();
            Globals.WriteWithColor($"FILE VIEWER V0.1.0 | {Globals.openFile.name}.{Globals.openFile.extension}", ConsoleColor.White, ConsoleColor.Black);
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

        public static void ImageViewer()
        {
            if (Globals.openFile == null) return;

            Clear();
            Globals.WriteWithColor($"IMAGE VIEWER V0.1.0 | {Globals.openFile.name}.{Globals.openFile.extension}", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");

            bool writen = false;

            for (int i = 0; i < Globals.openFile.content.Count(); i++)
            {
                char[] colors = Globals.openFile.content[i].ToCharArray();

                Write(" ");

                for (int j = 0; j < colors.Length; j++)
                {
                    if (colors[j] == ' ') continue;

                    int color;
                    try 
                    {
                        color = Convert.ToInt32(colors[j].ToString(), 16);
                    } catch 
                    {
                        Clear();
                        Globals.WriteError($"'{Globals.openFile.content[i]}' on line {i}, character {j} not recognized.");
                        return;
                    }

                    Pixel(color);
                    writen = true;
                }

                WriteLine(" ");
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

        public static void Pixel(int color)
        {
            BackgroundColor = (ConsoleColor)color;
            ForegroundColor = (ConsoleColor)color;

            Write("mm");

            ResetColor();
        }
    }

    class FileEditor
    {
        public static int mode = 0;
        public static int selectedLine = 0;

        public static void EditFile(string name, Directory[] filePath)
        {
            int idx = name.LastIndexOf('.');
            if (idx != -1) name = name[..idx];

            Globals.openFile = File.FindInChildren(name, filePath);
            if (Globals.openFile == null)
            {
                Globals.WriteError("No such file exists.");
                return;
            } else if (Globals.openFile.extension == "img")
            {
                Globals.WriteError("Cannot edit 'img' file. Change it back to 'txt' format to edit it.");
                return;
            }

            Refresh();

            bool exit = false;
            while (exit == false)
            {
                ConsoleKeyInfo keyInfo = ReadKey(true);
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedLine < 0) break;
                        selectedLine -= 1;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedLine >= Globals.openFile.content.Count()) break;
                        selectedLine += 1;
                        break;
                        
                    case ConsoleKey.Q:
                        exit = true;
                        break;
                        
                    case ConsoleKey.Enter:
                        LineEdit(selectedLine);
                        break;

                    case ConsoleKey.N:
                        Globals.openFile.content.Add(" ");
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
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;
            WriteLine(text);
            ResetColor();
        }

        static void LineEdit(int line)
        {
            if (Globals.openFile == null) return;
            if (line < 0 || line >= Globals.openFile.content.Count()) return;

            Clear();
            Globals.WriteWithColor($"FILE EDITOR V0.1.0 | {Globals.openFile.name}.{Globals.openFile.extension}", ConsoleColor.White, ConsoleColor.Black);
            WriteLine("");
            Write(line + " | ");

            string? newLine = ReadLine();
            if (newLine == null) newLine = " ";
            Globals.openFile.content[line] = newLine;
        }

        static void Refresh()
        {
            if (Globals.openFile == null) return;

            Clear();
            Globals.WriteWithColor($"FILE EDITOR V0.1.0 | {Globals.openFile.name}.{Globals.openFile.extension}", ConsoleColor.White, ConsoleColor.Black);
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

                if (selectedLine == i)
                {
                    WriteSelected(number + Globals.openFile.content[i]);

                } else
                {
                    WriteLine(number + Globals.openFile.content[i]);
                }
            }

            if (writen == false)
            {
                Write("0 | ");
            }
        }
    }
}