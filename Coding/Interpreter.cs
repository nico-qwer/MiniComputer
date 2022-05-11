using System;
using static System.Console;

namespace MiniComputer
{
    class Interpreter
    {
        static string[]? fileContent;
        static List<Variable> variables = new List<Variable>();
        static List<string> processes = new List<string>();
        static int line = 0;

        public static void Run()
        {
            if (Globals.openFile == null) return;

            fileContent = Globals.openFile.content.ToArray();

            for (line = 0; line < fileContent.Count(); line++)
            {
                char[] characters = fileContent[line].ToCharArray();
                for (int j = 0; j < characters.Length; j++)
                {
                    if (characters[j] == ' ') fileContent[line].Remove(0, 1);
                    else break;
                }

                string[] tokens = fileContent[line].Split(" ");

                if (processes.Count() > 0)
                {
                    if (tokens[0] == ";") processes.RemoveAt(processes.Count() - 1);
                    else if (tokens[0] == "if" && processes.Last() == "skip") processes.Add("skip");
                    else if (tokens[0] == "while" && processes.Last() == "skip") processes.Add("skip");
                    else if (Int32.TryParse(processes.Last(), out line)) { continue; }
                    else if (processes.Last() == "skip") continue;
                }

                switch (tokens[0])
                {
                    //Variable Creation
                    case "var":
                        if (tokens[1] == null || tokens[1] == "") { Globals.WriteError($"{line}: Variable must have name."); return; }

                        if (tokens[2] != "=" && tokens[3] == null || tokens[3] == "") { Globals.WriteError($"{line}: Variable must be assigned to."); return; }

                        for (int j = 0; j < variables.Count(); j++)
                        {
                            if (variables[j].name == tokens[1]) { Globals.WriteError($"{line}: Variable already declared."); return; }
                        }

                        Variable newVariable = new Variable(tokens[1], tokens.Skip(3).ToArray());

                        if (newVariable.type == "invalid")
                        {
                            Globals.WriteError($"{line}: Variable assignment invalid.");
                            break;
                        }

                        variables.Add(newVariable);
                        break;

                    case "out":
                        Output(0, tokens);
                        break;

                    case "if":
                        if (tokens.Length <= 1) { Globals.WriteError($"{line}: Arguments missing."); break; }

                        bool? valueI = isTrue(tokens);
                        if (valueI == null) { Globals.WriteError($"{line}: Condition invalid."); break; }

                        if (valueI == false) processes.Add("skip");
                        else if (valueI == true) processes.Add("go");

                        break;

                    case "while":
                        if (tokens.Length <= 1) { Globals.WriteError($"{line}: Arguments missing."); break; }

                        WriteLine("Hey!");

                        bool? valueW = isTrue(tokens);
                        if (valueW == null) { Globals.WriteError($"{line}: Condition invalid."); break; }

                        if (valueW == false) processes.Add("skip");
                        else if (valueW == true) processes.Add(line.ToString());

                        break;

                    case "":

                        break;

                    default:
                        break;
                }
            }

            Write("\n");
            variables = new List<Variable>();
            processes = new List<string>();
            line = 0;
        }

        static bool? isTrue(string[] tokens)
        {
            if (tokens.Length == 2)
            {
                bool? value = IsStringTrue(tokens.Last());
                return value;
            }
            else if (tokens.Length == 4 && tokens[3] == "==")
            {
                bool? value1 = IsStringTrue(tokens[2]);
                bool? value2 = IsStringTrue(tokens[4]);

                if (value1 == null || value2 == null) return null;

                return value1 == value2;
            }
            else if (tokens.Length == 4 && tokens[3] == "!=")
            {
                bool? value1 = IsStringTrue(tokens[2]);
                bool? value2 = IsStringTrue(tokens[4]);

                if (value1 == null || value2 == null) return null;

                return value1 != value2;
            }

            return null;
        }

        static void Output(int mod, string[] tokens)
        {
            if (tokens.Length < 2 + mod)
            {
                if (mod == 0) Write("\n");
                Globals.WriteError($"{line}: Argument(s) invalid.");
                return;
            }

            WriteLine(GetString(tokens.Skip(1).ToArray()));
        }

        public static string? GetInput()
        {
            return ReadLine();
        }

        static Variable? GetVariable(string name)
        {
            for (int j = 0; j < variables.Count(); j++)
            {
                if (variables[j].name != name) continue;
                return variables[j];
            }
            Globals.WriteError($"{line}: Variable not found.");
            return null;
        }

        static bool? IsStringTrue(string value)
        {
            if (value.StartsWith('$'))
            {
                Variable? referencedVar = GetVariable(value.Remove(0, 1));
                if (referencedVar == null) { Globals.WriteError($"{line}: Variable not found."); return null; }
                if (referencedVar.type != "bool") { Globals.WriteError($"{line}: Variable must be compared."); return null; }

                bool result;
                if (Boolean.TryParse(referencedVar.value, out result) == false) { Globals.WriteError($"{line}: Variable assigned improperly."); return null; }

                return result;
            }
            else
            {
                bool result;
                if (Boolean.TryParse(value, out result) == false) { Globals.WriteError($"{line}: Unregognized token '{value}'."); return null; }
                return result;
            }
        }

        public static string GetString(string[] toGetFrom)
        {
            string output = "";
            string newString = "";

            if (toGetFrom.Length == 0) return output;

            for (int i = 0; i < toGetFrom.Length; i++)
            {
                if (toGetFrom[i].StartsWith('$'))
                {
                    Variable? refVar = GetVariable(toGetFrom[i].Remove(0, 1));
                    if (refVar == null) continue;
                    output += refVar.value;
                }
                else if (newString == string.Empty && toGetFrom[i] == "get()")
                {
                    output += GetInput();
                }
                else if (toGetFrom[i].StartsWith('"'))
                {
                    if (toGetFrom[i].EndsWith('"')) output += toGetFrom[i].Substring(1, toGetFrom[i].Length - 2);
                    else newString = toGetFrom[i].Remove(0, 1);
                }
                else if (newString != string.Empty)
                {
                    if (toGetFrom[i].EndsWith('"')) { output += newString + " " + toGetFrom[i].Remove(toGetFrom[i].Length - 1, 1); newString = ""; }
                    else newString += " " + toGetFrom[i];
                }
            }

            if (output == string.Empty) Globals.WriteError("String is empty");
            return output;
        }

    }
}