using System;
using static System.Console;

namespace MiniComputer
{
    class Variable
    {
        public string name { get; private set; }
        public string type { get; private set; }
        public string value;

        public Variable(string newName, string newtype, string[] newValues)
        {
            name = newName;
            type = newtype;
            string? newStrValue = Interpreter.GetString(newValues);
            int? newIntValue = Interpreter.GetInt(newValues);
            float? newFloatValue = Interpreter.GetFloat(newValues);

            //Float test
            if (newFloatValue != null)
            {
                type = "float";
                if (newFloatValue != null) { float nonNull = (float)newFloatValue; value = nonNull.ToString(); }
                else { type = "invalid"; value = "null"; }
            }
            //Int test
            else if (newIntValue != null)
            {
                type = "int";
                if (newIntValue != null) { float nonNull = (float)newIntValue; value = nonNull.ToString(); }
                else { type = "invalid"; value = "null"; }
            }
            //Bool test
            else if (newValues[0] == "true" || newValues[0] == "false")
            {
                type = "bool";
                value = newValues[0];
            }
            //String test
            else if (newStrValue != null)
            {
                type = "string";
                value = newStrValue;
            }
            else
            {
                type = "invalid";
                value = "null";
            }
        }
    }
}