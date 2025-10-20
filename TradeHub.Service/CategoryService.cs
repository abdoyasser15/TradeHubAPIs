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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            var categoryDto = categories.Select(
                c => new CategoryDto
            {
                Name = c.Name,
                IsActive = c.IsActive
            }).ToList();
            return categoryDto;
        }
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await  _unitOfWork.Repository<Category>().GetById(id);
            if (category is null)
                return null;
            var categoryDto = new CategoryDto
            {
                Name = category.Name,
                IsActive = category.IsActive
            };
            return categoryDto;
        }
        public async Task<CategoryDto?> AddAsync(CategoryDto category)
        {
            if (category is null)
                return null;
            var newCategory = new Category
            {
                Name = category.Name,
                IsActive = category.IsActive
            };
            await _unitOfWork.Repository<Category>().AddAsync(newCategory);
            await _unitOfWork.CompleteAsync();
            return category;
        }
        public async Task<CategoryDto?> UpdateAsync(int id, CategoryDto category)
        {
            if (category is null)
                return null;
            var existingCategory = await _unitOfWork.Repository<Category>().GetById(id);
            if (existingCategory is null)
                return null;
            existingCategory.Name = category.Name;
            existingCategory.IsActive = category.IsActive;
            _unitOfWork.Repository<Category>().Update(existingCategory);
            await _unitOfWork.CompleteAsync();
            return category;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;
            var category = await _unitOfWork.Repository<Category>().GetById(id);
            if (category is null)
                return false;
             _unitOfWork.Repository<Category>().DeleteAsync(category);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetActiveAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().FindAsync(C=>C.IsActive==true);
            var categoryDto = categories.Select(
                c => new CategoryDto
                {
                    Name = c.Name,
                    IsActive = c.IsActive
                }).ToList();
            return categoryDto;
        }
    }
}
