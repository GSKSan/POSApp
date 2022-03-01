using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.ConsoleManager
{
    public interface IConsoleManager
    {
        void WriteLine(string value);
        ConsoleKeyInfo ReadKey();

        string ReadLine();
    }
}
