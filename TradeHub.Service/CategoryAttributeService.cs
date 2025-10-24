using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.CategoryAttributeSpec;

namespace TradeHub.Service
{
    public class CategoryAttributeService : ICategoryAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryAttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddCategoryAttribute(CategoryAttributeCreateDto dto)
        {
            if(dto == null)
                return false;
            var categoryAttribute = new CategoryAttribute
            {
                Name = dto.Name,
                DataType = dto.DataType,
                IsRequired = dto.IsRequired,
                CategoryId = dto.CategoryId
            };
            await _unitOfWork.Repository<CategoryAttribute>().AddAsync(categoryAttribute);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        

        public async Task<IReadOnlyList<CategoryAttributeDto>> GetAllCategoryAttribute()
        {
            var spec = new CategoryAttributeWithCategorySpecification();
            var categoryAttributes = await _unitOfWork.Repository<CategoryAttribute>().GetAllSpecificationsAsync(spec);
            var categoryAttributeDtos = categoryAttributes.Select(ca => new CategoryAttributeDto
            {
                Id = ca.Id,
                Name = ca.Name,
                DataType = ca.DataType,
                IsRequired = ca.IsRequired,
                CategoryId = ca.CategoryId,
                CategoryName = ca.Category != null ? ca.Category.Name : string.Empty
            }).ToList();
            return categoryAttributeDtos;
        }
        public async Task<CategoryAttributeDto> GetCategoryAttributeById(int id)
        {
            var spec = new CategoryAttributeWithCategorySpecificationById(id);
            var categoryAttribute = await  _unitOfWork.Repository<CategoryAttribute>().GetWithSpecAsync(spec);
            if (categoryAttribute == null)
                return null;
            var dto = new CategoryAttributeDto
            {
                Id = categoryAttribute.Id,
                Name = categoryAttribute.Name,
                DataType = categoryAttribute.DataType,
                IsRequired = categoryAttribute.IsRequired,
                CategoryId = categoryAttribute.CategoryId,
                CategoryName = categoryAttribute.Category != null ? categoryAttribute.Category.Name : string.Empty
            };
            return dto;
        }
        public async Task<bool> UpdateCategoryAttribute(int id, CategoryAttributeUpdateDto dto)
        {
            var categoryAttribute = await _unitOfWork.Repository<CategoryAttribute>().GetById(id);
            if (categoryAttribute == null)
                return false;
            categoryAttribute.Name = dto.Name;
            categoryAttribute.DataType = dto.DataType;
            categoryAttribute.IsRequired = dto.IsRequired;
            categoryAttribute.CategoryId = dto.CategoryId;
            _unitOfWork.Repository<CategoryAttribute>().Update(categoryAttribute);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public async Task<bool> DeleteCategoryAttribute(int id)
        {
            var categoryAttribute = await _unitOfWork.Repository<CategoryAttribute>().GetById(id);
            if (categoryAttribute == null)
                return false;
            _unitOfWork.Repository<CategoryAttribute>().DeleteAsync(categoryAttribute);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
