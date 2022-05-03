using System;
using System.Text;

using static System.Console;

namespace MiniComputer
{
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
            }

            Refresh();

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
                        if (selectedLine >= Globals.openFile.content.Count() - 1) break;
                        selectedLine += 1;
                        break;

                    case ConsoleKey.Q:
                        exit = true;

                        for (int i = Globals.openFile.content.Count() - 1; i >= 0; i--)
                        {
                            if (Globals.openFile.content[i] == "") Globals.openFile.content.RemoveAt(i);
                            else break;
                        }

                        break;
                    case ConsoleKey.S:
                        SaveSystem.SaveFile(Globals.openFile);
                        break;

                    case ConsoleKey.Enter:
                        LineEdit(selectedLine);
                        break;

                    case ConsoleKey.N:
                        Globals.openFile.content.Add("");
                        break;

                    case ConsoleKey.Backspace:
                        if (Globals.openFile.content.Count() > 0)
                        {
                            Globals.openFile.content.RemoveAt(Globals.openFile.content.Count - 1);
                        }
                        break;

                    default:
                        break;
                }

                Refresh();
            }

            SaveSystem.SaveFile(Globals.openFile);

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

            string? newLine = Globals.openFile.content[line];

            bool exit = false;
            int charCount = Globals.openFile.content[line].Length;
            int insertChar = charCount;

            Clear();
            Globals.WriteWithColor($"FILE EDITOR V0.1.0 | {Globals.openFile.name}.{Globals.openFile.extension}", ConsoleColor.White, ConsoleColor.Black);
            Write(" \n" + line + " | ");
            RefreshLine(line, insertChar, newLine);

            while (exit == false)
            {
                ConsoleKeyInfo newLetter = ReadKey(true);

                switch (newLetter.Key)
                {
                    case ConsoleKey.Backspace:
                        if (charCount == 0) continue;

                        newLine = newLine.Remove(insertChar - 1, 1);

                        charCount--;
                        insertChar--;

                        RefreshLine(line, insertChar - 1, newLine);

                        break;

                    case ConsoleKey.Enter:
                        exit = true;
                        break;

                    case ConsoleKey.LeftArrow:

                        if (Console.CursorLeft <= line.ToString().Length + 4) continue;
                        Console.CursorLeft = Console.CursorLeft - 1;
                        insertChar--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (Console.CursorLeft >= line.ToString().Length + 4 + newLine.Length - 1) continue;
                        Console.CursorLeft = Console.CursorLeft + 1;
                        insertChar++;
                        break;

                    default:

                        string newLine_ = newLine.Insert(insertChar, newLetter.KeyChar.ToString());
                        newLine = newLine_;

                        RefreshLine(line, insertChar, newLine);

                        charCount++;
                        insertChar++;
                        break;
                }
            }

            if (newLine == null) newLine = "";

            char[] characters = newLine.ToCharArray();
            for (int i = characters.Length - 1; i >= 0; i--)
            {
                if (characters[i] == ' ') newLine.Remove(newLine.Length - 1); //Write(i);}
                else break;
            }

            Globals.openFile.content[line] = newLine;
            WriteLine(newLine);
        }

        static void RefreshLine(int line, int cursor, string text)
        {
            if (Globals.openFile == null) return;

            CursorLeft = 0;
            Write(new string(' ', BufferWidth));
            CursorTop = CursorTop - 1;

            Write(line + " | ");
            Write(text);
            CursorLeft = cursor + line.ToString().Length + 4;
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

                }
                else
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