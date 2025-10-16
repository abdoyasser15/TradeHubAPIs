using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class OtpService : IOtpService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public OtpService(IUnitOfWork unitOfWork , INotificationService notificationService )
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<bool> GenerateOtpAsync(string PhoneOrEmail)
        {
            var code = new Random().Next(10000, 99999).ToString();
            var otp = new OtpCode
            {
                UserId = PhoneOrEmail,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.Repository<OtpCode>().AddAsync(otp);
            await _unitOfWork.CompleteAsync();

            await _notificationService.SendAsync(PhoneOrEmail, $"Your TradeHub OTP code is: {code}");

            return true;
        }

        public async Task<bool> ValidateOtpAsync(string phoneOrEmail, string code)
        {
            var otpRepo = _unitOfWork.Repository<OtpCode>();
            var otp = (await otpRepo.FindAsync(x => x.UserId == phoneOrEmail && !x.IsUsed))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (otp == null)
                return false;

            if (otp.Expiration < DateTime.UtcNow)
                return false;

            if (otp.Code != code)
                return false;

            otp.IsUsed = true;
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
