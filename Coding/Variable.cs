using System;
using static System.Console;

namespace MiniComputer
{
    class Variable
    {
        public string name { get; private set; }
        public string type { get; private set; }
        public string value;

        public Variable(string newName, string newValue)
        {
            name = newName;

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
            else if (newValue.StartsWith('"') && newValue.EndsWith('"'))
            {
                type = "string";
                newValue = newValue.Substring(1, newValue.Length - 2);
            }
            else
            {
                type = "invalid";
            }

            value = newValue;
        }
    }
}