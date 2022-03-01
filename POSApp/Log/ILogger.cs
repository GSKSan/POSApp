using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.Log
{
    public interface ILogger
    {
        void LogMessage(string message);
    }
}
