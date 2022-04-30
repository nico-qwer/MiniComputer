using System;
using static System.Console;

namespace MiniComputer
{
    class Interpreter
    {
        public static void Run()
        {
            if (Globals.openFile == null) return;

            List<Variable> variables = new List<Variable>();

            for (int line = 0; line < Globals.openFile.content.Count(); line++)
            {
                string unIndentedLine = Globals.openFile.content[line].Replace("\t", "");
                string[] tokens = unIndentedLine.Split(" ");

                /*for (int j = 0; j < tokens.Length; j++)
				{
					WriteLine(tokens[j]);
				}*/

                switch (tokens[0])
                {
                    //Variable Creationiable Creation
                    case "var":
                        if (tokens[1] == null || tokens[1] == "")
                        {
                            Globals.WriteError($"{line}: Variable must have name.");
                            return;
                        }

                        if (tokens[2] != "=" && tokens[3] == null || tokens[3] == "")
                        {
                            Globals.WriteError($"{line}: Variable must be assigned to.");
                            return;
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
                        Output(line, 0, tokens, variables);
                        break;

                    case "if":



                    case "":

                        break;

                    default:
                        break;
                }
            }

            Write("\n");
        }

        static void Output(int line, int mod, string[] tokens, List<Variable> variables)
        {
            if (tokens.Length < 2 + mod)
            {
                Globals.WriteError($"{line}: Argument(s) invalid.");

                //If Stringf String
                if (tokens[1 + mod].StartsWith('"') && tokens[1 + mod].EndsWith('"'))
                {
                    Write(tokens[1 + mod].Substring(1, tokens[1 + mod].Length - 2));

                    if (tokens.Length > 2 + mod && tokens[2 + mod] == "+")
                    {
                        Output(line, 2 + mod, tokens, variables);
                    }

                    //If variablevariable
                    else if (tokens[1 + mod].StartsWith('$'))
                    {
                        bool found = false;
                        for (int j = 0; j < variables.Count(); j++)
                        {
                            if (variables[j].name != tokens[1 + mod].Remove(0, 1)) continue;
                            Write(variables[j].value);
                            found = true;
                            break;
                        }
                        if (found == false) { Globals.WriteError($"{line}: Variable not found."); return; }

                        if (tokens.Length > 2 + mod && tokens[2 + mod] == "+")
                        {
                            Output(line, 2 + mod, tokens, variables);
                        }

                        //If nothing nothing
                        else { Globals.WriteError($"{line}: Argument {tokens[1 + mod]} invalid."); return; }
                    }
                }
            }
        }
    }
}