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

                /*for (int j = 0; j < tokens.Length; j++)
				{
					WriteLine(tokens[j]);
				}*/

                if (tokens[0] == ";")
                {
                    processes.RemoveAt(processes.Count() - 1);
                }

                if (processes.Count() > 0 && processes.Last() == "skip") continue;

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

                        Variable newVariable = new Variable(tokens[1], tokens[3]);

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
                        if (tokens.Length <= 1) { Globals.WriteError($"{line}: If arguments missing."); break; }

                        bool? valueI = isTrue(tokens);
                        if (valueI == null) { Globals.WriteError($"{line}: Variable not found."); break; }

                        if (valueI == false) processes.Add("skip");
                        else if (valueI == true) processes.Add("go");

                        break;

                    case "while":
                        if (tokens.Length <= 1) { Globals.WriteError($"{line}: While arguments missing."); break; }

                        bool? valueW = isTrue(tokens);
                        if (valueW == null) { Globals.WriteError($"{line}: Variable not found."); break; }

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

                return value1 == value2;
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
            //If String
            if (tokens[1 + mod].StartsWith('"') && tokens[1 + mod].EndsWith('"'))
            {
                Write(tokens[1 + mod].Substring(1, tokens[1 + mod].Length - 2));

                if (tokens.Length > 2 + mod && tokens[2 + mod] == "+")
                {
                    Output(2 + mod, tokens);
                }
            }
            //If variable
            else if (tokens[1 + mod].StartsWith('$'))
            {

                Variable? referencedVar = GetVariable(tokens[1 + mod].Remove(0, 1));
                if (referencedVar == null) { Globals.WriteError($"{line}: Variable not found."); return; }

                Write(referencedVar.value);

                if (tokens.Length > 2 + mod && tokens[2 + mod] == "+")
                {
                    Output(2 + mod, tokens);
                }
            }
            //If nothing
            else { Globals.WriteError($"{line}: Argument {tokens[1 + mod]} invalid."); return; }
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

        static bool? StringToBool(string orgString)
        {
            if (orgString == "true")
            {
                return true;
            }
            else if (orgString == "false")
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        static bool? IsStringTrue(string value)
        {
            if (value.StartsWith('$'))
            {
                Variable? referencedVar = GetVariable(value.Remove(0, 1));
                if (referencedVar == null) { Globals.WriteError($"{line}: Variable not found."); return null; }
                if (referencedVar.type != "bool") { Globals.WriteError($"{line}: Variable must be compared."); return null; }

                bool? result = StringToBool(referencedVar.value);
                if (result == null) { Globals.WriteError($"{line}: Variable assigned improperly."); return null; }

                return result;
            }
            else
            {
                bool? result = StringToBool(value);
                if (result == null) { Globals.WriteError($"{line}: Unregognized token '{value}'."); return null; }
                return result;
            }
        }
    }
}