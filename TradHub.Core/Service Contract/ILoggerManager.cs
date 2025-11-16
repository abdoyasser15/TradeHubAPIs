using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Service_Contract
{
    public interface ILoggerManager
    {
        void LogInfo(string message, params object[] args);
        void LogWarn(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
    }
}
