using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.ConsoleManager
{
    public class ConsoleManager : IConsoleManager
    {
        public void DeleteChar()
        {
            Console.Write("\b");
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);   
        }
    }
}
