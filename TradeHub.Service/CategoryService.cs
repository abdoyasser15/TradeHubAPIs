using Serilog;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IImageService _imageService;

        public CategoryService(IUnitOfWork unitOfWork, ILoggerManager logger, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imageService;
        }
        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
        {
            try
            {
                _logger.LogInfo("Fetching all categories from the database.");

                var categories = await _unitOfWork.Repository<Category>().GetAllAsync();

                _logger.LogInfo($"Fetched {categories.Count} Categories from the database.");

                return categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"An error occurred while retrieving categories: {ex.Message}");
                throw;
            }
        }
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInfo($"Fetching category with ID {id} from the database.");

                var category = await _unitOfWork.Repository<Category>().GetById(id);

                return category == null ? null : new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving category with ID {id}: {ex.Message}");
                throw;
            }
        }
        public async Task<CategoryDto?> AddAsync(CreateCategoryDto categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarn("Add failed: Category data is null.");
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                _logger.LogWarn("Add failed: Category name is empty.");
                throw new ArgumentException("Category name is required.");
            }

            try
            {
                var categoryName = categoryDto.Name.Trim();

                var exists = await _unitOfWork.Repository<Category>()
                    .FindAsync(x => x.Name.ToLower() == categoryName.ToLower());

                if (exists.Any())
                {
                    _logger.LogWarn("Duplicate category name: {Name}", categoryName);
                    throw new DuplicateNameException($"Category '{categoryName}' already exists.");
                }

                var imageUrl = await _imageService.UploadImageAsync(
                    categoryDto.Image!,
                    "images/categories"
                );

                var newCategory = new Category
                {
                    Name = categoryName,
                    ImageUrl = imageUrl
                };

                await _unitOfWork.Repository<Category>().AddAsync(newCategory);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Category '{Name}' added successfully", categoryName);

                return new CategoryDto
                {
                    Id = newCategory.Id,
                    Name = newCategory.Name,
                    ImageUrl = newCategory.ImageUrl
                };
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding category: {Name}", categoryDto.Name);
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
                var category = await _unitOfWork.Repository<Category>().GetById(id);
                if (category is null)
                {
                    _logger.LogWarn("Delete failed: Category with Id={Id} not found.", id);
                    return false;
                }
                _unitOfWork.Repository<Category>().DeleteAsync(category);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while Deleting category with Id={Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Deleting category with Id={Id}", id);
                throw;
            }
        }
        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto)
        {
            if (id <= 0)
            {
                _logger.LogWarn("Invalid category Id: {Id}", id);
                throw new ArgumentException("Category Id must be greater than zero.");
            }

            if (categoryDto is null)
            {
                _logger.LogWarn("Update failed: UpdateCategoryDto is null.");
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                _logger.LogWarn("Update failed: Category name is empty.");
                throw new ArgumentException("Category name is required.");
            }
            try
            {
                var categoryName = categoryDto.Name.Trim();

                var existingCategory = await _unitOfWork.Repository<Category>().GetById(id);

                if (existingCategory is null)
                {
                    _logger.LogWarn("Update failed: Category with Id={Id} not found.", id);
                    return null;
                }

                var duplicate = await _unitOfWork.Repository<Category>()
                    .FindAsync(x => x.Name.ToLower() == categoryName.ToLower() && x.Id != id);

                if (duplicate.Any())
                {
                    _logger.LogWarn("Duplicate category name during update: {Name}", categoryName);
                    throw new DuplicateNameException($"Category '{categoryName}' already exists.");
                }

                existingCategory.Name = categoryName;

                if (categoryDto.Image is not null)
                {
                    if (!string.IsNullOrWhiteSpace(existingCategory.ImageUrl))
                    {
                        _imageService.DeleteImage(existingCategory.ImageUrl);
                    }

                    existingCategory.ImageUrl = await _imageService.UploadImageAsync(
                        categoryDto.Image,
                        "images/categories"
                    );
                }

                _unitOfWork.Repository<Category>().Update(existingCategory);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Category with Id={Id} updated successfully", id);

                return new CategoryDto
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name,
                    ImageUrl = existingCategory.ImageUrl
                };
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogWarn("Duplicate error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating category with Id={Id}", id);
                throw;
            }
        }
    }
}
