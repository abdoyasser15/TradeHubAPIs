using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Service_Contract
{
    public interface IOtpService
    {
        Task<bool> GenerateOtpAsync(string phoneOrEmail);
        Task<bool> ValidateOtpAsync(string phoneOrEmail, string code);
    }
}
