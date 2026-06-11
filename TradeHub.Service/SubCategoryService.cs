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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public SubCategoryService(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IReadOnlyList<SubCategoryDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInfo("Fetching all sub categories from the database.");

                var categories = await _unitOfWork.Repository<SubCategory>().GetAllAsync(x=>x.Category);

                _logger.LogInfo($"Fetched {categories.Count} subcategories from the database.");

                return categories.Select(
                     c => new SubCategoryDto
                     {
                         Id = c.SubCategoryId,
                         Name = c.Name,
                         CategoryName = c.Category.Name,
                         IsActive = c.IsActive
                     }).ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all subcategories.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all categories.");
                throw;
            }
        }
        public async Task<SubCategoryDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarn("Invalid category Id: {Id}", id);
                    throw new ArgumentException("Category Id must be greater than zero.");
                }
                _logger.LogInfo("Fetching category with Id={Id}", id);
                var category = await _unitOfWork.Repository<SubCategory>().GetById(id, x => x.Category);
                if (category is null)
                {
                    _logger.LogWarn("Category with Id={Id} not found", id);
                    return null;
                }
                _logger.LogInfo("Category with Id={Id} Fetched Successfully", id);
                return new SubCategoryDto
                {
                    Id = category.SubCategoryId,
                    Name = category.Name,
                    IsActive = category.IsActive,
                    CategoryName = category.Category.Name
                };
            }
            catch (AbandonedMutexException ex)
            {
                _logger.LogError(ex, "Abandoned mutex error while fetching category with Id={Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category with Id={Id}", id);
                throw;
            }
        }
        public async Task<SubCategoryDto?> AddAsync(CreateSubCategoryDto category)
        {
            try
            {
                var exists = await _unitOfWork.Repository<SubCategory>()
                    .FindAsync(x => x.Name == category.Name);

                if (exists.Any())
                {
                    _logger.LogWarn("Duplicate category name: {Name}", category.Name);
                    throw new DuplicateNameException($"Category '{category.Name}' already exists.");
                }
                var newCategory = new SubCategory
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    CategoryId = category.CategoryId
                };

                await _unitOfWork.Repository<SubCategory>().AddAsync(newCategory);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Category '{Name}' added successfully", category.Name);

                var parentCategory = await _unitOfWork.Repository<Category>().GetById(category.CategoryId);
                
                return new SubCategoryDto
                {
                    Id = newCategory.SubCategoryId,
                    Name = newCategory.Name,
                    IsActive = newCategory.IsActive,
                    CategoryName = parentCategory.Name
                };
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding category: {Name}", category?.Name);
                throw;
            }
        }
        public async Task<SubCategoryDto?> UpdateAsync(int id, SubCategoryDto category)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarn("Invalid category Id: {Id}", id);
                    throw new ArgumentException("Category Id must be greater than zero.");
                }
                if(category is null)
                {
                    _logger.LogWarn("Update failed: CategoryDto is null.");
                    throw new ArgumentNullException("Category data cannot be null.");
                }

                _logger.LogInfo("Fetching category for update. Id={Id}", id);

                var existingCategory = await _unitOfWork.Repository<SubCategory>().GetById(id);
                if (existingCategory is null)
                {
                    _logger.LogWarn("Update failed: Category with Id={Id} not found.", id);
                    return null;
                }

                var duplicate = await _unitOfWork.Repository<SubCategory>()
                    .FindAsync(x => x.Name == category.Name && x.SubCategoryId != id);

                if (duplicate.Any())
                {
                    _logger.LogWarn("Duplicate category name during update: {Name}", category.Name);
                    throw new DuplicateNameException($"Category '{category.Name}' already exists.");
                }
                existingCategory.Name = category.Name;
                existingCategory.IsActive = category.IsActive;

                _logger.LogInfo("Updating category with Id={Id}", id);
                _unitOfWork.Repository<SubCategory>().Update(existingCategory);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Category with Id={Id} updated successfully", id);
                return new SubCategoryDto
                {
                    Name = existingCategory.Name,
                    IsActive = existingCategory.IsActive
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while updating category with Id={Id}", id);
                throw;
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate update error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating category with Id={Id}", id);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarn("Invalid category Id: {Id}", id);
                    throw new ArgumentException("Category Id must be greater than zero.");
                }
                _logger.LogInfo("Fetching category for deletion. Id={Id}", id);
                var category = await _unitOfWork.Repository<SubCategory>().GetById(id);
                if (category is null)
                {
                    _logger.LogWarn("Delete failed: Category with Id={Id} not found.", id);
                    return false;
                }
                _unitOfWork.Repository<SubCategory>().DeleteAsync(category);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while Deleting category with Id={Id}", id);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting category with Id={Id}", id);
                throw;
            }
        }
        public async Task<IReadOnlyList<SubCategoryDto>> GetActiveAsync()
        {
            try
            {
                _logger.LogInfo("Fetching active categories...");
                var categories = await _unitOfWork.Repository<SubCategory>().FindAsync(C => C.IsActive == true);

                _logger.LogInfo("Fetched {Count} active categories.", categories?.Count ?? 0);

                return categories.Select(
                    c => new SubCategoryDto
                    {
                        Name = c.Name,
                        IsActive = c.IsActive,
                        CategoryName = c.Category.Name
                    }).ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error while fetching active categories.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active categories.");
                throw;
            }
        }
    }
}
