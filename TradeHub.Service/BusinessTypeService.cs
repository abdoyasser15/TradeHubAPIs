using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        private readonly ILoggerManager _logger;

        public BusinessTypeService(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IReadOnlyList<BusinessTypeDto>> GetBusinessTypesAsync()
        {
            try
            {
                _logger.LogInfo("Fetching all Business Types from the database.");

                var business = await _unitOfWork.Repository<BusinessType>().GetAllAsync();

                _logger.LogInfo("Fetched {businessCount} Business Types from the database.",business.Count);
                return business.Select(b => new BusinessTypeDto
                {
                    BusinessName = b.Name,
                    IsActive = b.IsActive
                }).ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all categories.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Business Types");
                throw;
            }
        }
        public async Task<BusinessTypeDto> GetBusinessTypeByIdAsync(int id)
        {
            try
            {
                if (id <= 0) {
                    _logger.LogWarn("Invalid Business Type Id: {Id}", id);
                    throw new ArgumentException("Invalid Business Type Id");
                }
                _logger.LogInfo("Fetching Business Type with Id: {Id}", id);
                var business = await _unitOfWork.Repository<BusinessType>().GetById(id);
                if (business is null)
                {
                    _logger.LogWarn("Business Type with Id: {Id} not found", id);
                    throw new KeyNotFoundException("Business Type Not Found");
                }
                _logger.LogInfo("Business Type with Id: {Id} fetched successfully", id);
                return new BusinessTypeDto
                {
                    BusinessName = business.Name,
                    IsActive = business.IsActive
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: {Message}", ex.Message);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Not found error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Business Type with Id: {Id}", id);
                throw;
            }
        }
        public async Task<BusinessTypeDto?> AddBusinessTypeAsync(BusinessTypeDto businessType)
        {
            try
            {
                var existingBusinessType = await _unitOfWork.Repository<BusinessType>()
                    .FindAsync(bt => bt.Name.ToLower() == businessType.BusinessName.ToLower());
                if (existingBusinessType.Any())
                {
                    _logger.LogWarn("Business Type with name: {BusinessName} already exists", businessType.BusinessName);
                    throw new DuplicateNameException($"Category '{businessType.BusinessName}' already exists.");
                }

                if (businessType is null)
                {
                    _logger.LogWarn("Attempted to add a null Business Type");
                    return null;
                }
                var newBusinessType = new BusinessType
                {
                    Name = businessType.BusinessName,
                    IsActive = businessType.IsActive
                };
                await _unitOfWork.Repository<BusinessType>().AddAsync(newBusinessType);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Business Type '{BusinessName}' added successfully", businessType.BusinessName);
                return businessType;
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding Business Type: {BusinessName}", businessType?.BusinessName);
                throw;
            }
        }
        public async Task<BusinessTypeDto?> UpdateBusinessTypeAsync(int Id, BusinessTypeDto businessType)
        {
            try
            {
                if (Id <= 0)
                {
                    _logger.LogWarn("Invalid Business Type Id: {Id}", Id);
                    throw new ArgumentException("Invalid Business Type Id");
                }
                if (businessType is null)
                {
                    _logger.LogWarn("Attempted to update a null Business Type");
                    return null;
                }
                _logger.LogInfo("Fetching Business Type for update. Id={Id}", Id);
                var existingBusinessType = await _unitOfWork.Repository<BusinessType>().GetById(Id);
                if (existingBusinessType is null)
                {
                    _logger.LogWarn("Business Type with Id: {Id} not found for update", Id);
                    return null;
                }
                var duplicateCheck = await _unitOfWork.Repository<BusinessType>()
                    .FindAsync(bt => bt.Name.ToLower() == businessType.BusinessName.ToLower() && bt.BusinessTypeId != Id);
                if (duplicateCheck.Any())
                {
                    _logger.LogWarn("Business Type with name: {BusinessName} already exists", businessType.BusinessName);
                    throw new DuplicateNameException($"Category '{businessType.BusinessName}' already exists.");
                }
                existingBusinessType.Name = businessType.BusinessName;
                existingBusinessType.IsActive = businessType.IsActive;
                _logger.LogInfo("Updating Business Type with Id: {Id}", Id);
                _unitOfWork.Repository<BusinessType>().Update(existingBusinessType);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Business Type with Id: {Id} updated successfully", Id);
                return businessType;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: {Message}", ex.Message);
                throw;
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Business Type with Id: {Id}", Id);
                throw;
            }
        }
        public async Task<bool> DeleteBusinessTypeAsync(int id)
        {
            try
            {
                if( id <= 0)
                {
                    _logger.LogWarn("Invalid Business Type Id: {Id}", id);
                    throw new ArgumentException("Invalid Business Type Id");
                }
                _logger.LogInfo("Fetching Business Type for deletion. Id={Id}", id);
                var businessType = await _unitOfWork.Repository<BusinessType>().GetById(id);
                if (businessType is null)
                {
                    _logger.LogWarn("Business Type with Id: {Id} not found for deletion", id);
                    return false;
                }
                _unitOfWork.Repository<BusinessType>().DeleteAsync(businessType);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Business Type with Id: {Id}", id);
                throw;
            }
        }
    }
}
