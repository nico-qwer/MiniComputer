using System;
using static System.Console;

namespace MiniComputer
{
    class Variable
    {
        public string name { get; private set; }
        public string type { get; private set; }
        public string value;

        public Variable(string newName, string[] newValues)
        {
            name = newName;
            string newValue = Interpreter.GetString(newValues);

            //Negativity Removal
            string absValue = newValue;
            char[] valueChars = newValue.ToCharArray();
            if (valueChars[0] == '-')
            {
                absValue.Remove(0, 1);
            }

            //Int test
            if (float.TryParse(newValue, out _) || float.TryParse(newValue.Replace('.', ','), out _))
            {
                type = "number";
            }
            //Bool test
            else if (newValue == "true" || newValue == "false")
            {
                type = "bool";
            }
            //String default
            else
            {
                type = "string";
            }

            value = newValue;
        }
    }
}