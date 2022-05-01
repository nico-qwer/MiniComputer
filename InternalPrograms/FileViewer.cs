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
            else if (Globals.openFile.extension == "cod")
            {
                Interpreter.Run();
                return;
            }
            else
            {
                TextViewer();
                return;
            }
        }

        public static void TextViewer()
        {
            if (Globals.openFile == null) return;
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
                    if (colors[j] == ';') { WriteLine(" "); continue; }

                    int color;
                    try
                    {
                        color = Convert.ToInt32(colors[j].ToString(), 16);

                    }
                    catch
                    {
                        Clear();
                        Globals.WriteError($"'{Globals.openFile.content[i]}' on line {i}, character {j} not recognized.");
                        return;
                    }

                    Pixel(color);
                    writen = true;
                }
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

        public static void CodeExecuter()
        {
            if (Globals.openFile == null) return;




            for (int i = 0; i < Globals.openFile.content.Count(); i++)
            {
                string[] tokens = Globals.openFile.content[i].Split(" ");

                switch (tokens[0])
                {
                    case "var":

                        break;

                    default:

                        break;
                }
            }
        }

        public static void Pixel(int color)
        {
            BackgroundColor = (ConsoleColor)color;
            ForegroundColor = (ConsoleColor)color;

            Write("mm");

            ResetColor();
        }
    }
}