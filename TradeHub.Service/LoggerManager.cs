using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger<LoggerManager> _logger;

        public LoggerManager(ILogger<LoggerManager> logger)
        {
            _logger = logger;
        }
        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message,args);
        }

        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message,args);
        }

        public void LogWarn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
