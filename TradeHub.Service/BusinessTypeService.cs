using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class BusinessTypeService : IBusinessTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusinessTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<BusinessTypeDto>> GetBusinessTypesAsync()
        {
            var business = await _unitOfWork.Repository<BusinessType>().GetAllAsync();
            var businessDto = business.Select(b => new BusinessTypeDto
            {
                BusinessName = b.Name,
                IsActive = b.IsActive
            }).ToList();
            return businessDto;
        }
        public async Task<BusinessTypeDto> GetBusinessTypeByIdAsync(int id)
        {
            var business =  await _unitOfWork.Repository<BusinessType>().GetById(id);
            var businessDto = new BusinessTypeDto
            {
                BusinessName = business.Name,
                IsActive = business.IsActive
            };
            return businessDto;
        }
        public async Task<BusinessTypeDto?> AddBusinessTypeAsync(BusinessTypeDto businessType)
        {
            if (businessType is null)
                 return null;
            var newBusinessType = new BusinessType
            {
                Name = businessType.BusinessName,
                IsActive = businessType.IsActive
            };
            await _unitOfWork.Repository<BusinessType>().AddAsync(newBusinessType);
            await _unitOfWork.CompleteAsync();
            return businessType;
        }
        public async Task<BusinessTypeDto?> UpdateBusinessTypeAsync(int Id,BusinessTypeDto businessType)
        {
            if (businessType is null)
                return null;
            var existingBusinessType = await _unitOfWork.Repository<BusinessType>().GetById(Id);
            if (existingBusinessType is null)
                return null;
            existingBusinessType.Name = businessType.BusinessName;
            existingBusinessType.IsActive = businessType.IsActive;
            _unitOfWork.Repository<BusinessType>().Update(existingBusinessType);
            await _unitOfWork.CompleteAsync();
            return businessType;
        }
        public async Task<bool> DeleteBusinessTypeAsync(int id)
        {
            var businessType = await _unitOfWork.Repository<BusinessType>().GetById(id);
            if (businessType is null)
                return false;
            _unitOfWork.Repository<BusinessType>().DeleteAsync(businessType);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
