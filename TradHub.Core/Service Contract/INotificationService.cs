using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Service_Contract
{
    public interface INotificationService
    {
        Task SendAsync(string to, string message, string subject = null);
    }
}
