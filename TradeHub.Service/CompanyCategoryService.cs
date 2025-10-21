using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Service.Errors;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.CompanyCategorySepc;

namespace TradeHub.Service
{
    public class CompanyCategoryService : ICompanyCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddAsync(CompanyCategoryCreateDto dto)
        {
            var company = await _unitOfWork.Repository<Company>()
                .GetById(dto.CompanyId);
            if (company is null)
                throw new ApiValidationException(new[] { "Company not found" });

            var category = await _unitOfWork.Repository<Category>().GetById(dto.CategoryId);
            if (category is null)
                throw new ApiValidationException(new[] { "Category not found" });
            var exists = await _unitOfWork.Repository<CompanyCategory>()
                .AnyAsync(cc => cc.CompanyId == dto.CompanyId && cc.CategoryId == dto.CategoryId);
            if (exists)
                return false;

            var companyCategory = new CompanyCategory
            {
                CompanyId = dto.CompanyId,
                CategoryId = dto.CategoryId
            };
            await _unitOfWork.Repository<CompanyCategory>().AddAsync(companyCategory);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<IReadOnlyList<CompanyCategoryDto>> GetByCompanyIdAsync(Guid companyId)
        {
            var spec = new CompanyCategoryByCompanyIdSpecification(companyId);
            var companyCategories = await _unitOfWork.Repository<CompanyCategory>()
                .GetAllSpecificationsAsync(spec);
            if (!companyCategories.Any())
                return new List<CompanyCategoryDto>();
            var dtos = companyCategories.Select(cc => new CompanyCategoryDto
            {
                CompanyName = cc.Company.BusinessName,
                CategoryName = cc.Category.Name,
            }).ToList();
            return dtos;
        }
        public async Task<bool> RemoveAsync(Guid companyId, int categoryId)
        {
            var company = await _unitOfWork.Repository<Company>()
                .GetById(companyId);
            if (company is null)
                throw new ApiValidationException(new[] { "Company not found" });

            var category = await _unitOfWork.Repository<Category>().GetById(categoryId);
            if (category is null)
                throw new ApiValidationException(new[] { "Category not found" });

            var companyCategory = await _unitOfWork.Repository<CompanyCategory>()
                .FirstOrDefaultAsync(cc => cc.CompanyId == companyId && cc.CategoryId == categoryId);
            if(companyCategory is null)
                return false;
            _unitOfWork.Repository<CompanyCategory>().DeleteAsync(companyCategory);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
