using BookStore.Application.Common.Helpers;
using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Slug = SlugHelper.GenerateSlug(dto.Slug)
            };

            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null)
                return;

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CategoryDetailsDto>> Filter(string search)
        {
            var filteredCategories = await _dbContext.Categories
                .Where(x => x.Name.Contains(search))
                .Select(x => new CategoryDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .ToListAsync();

            return filteredCategories;
        }

        public async Task<List<CategoryDetailsDto>> GetAllAsync()
        {
            var categories = await _dbContext.Categories
                .Select(x => new CategoryDetailsDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryDetailsDto?> GetByIdAsync(int id)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(category is null)
                return null;

            var categoryDto = new CategoryDetailsDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };

            return categoryDto;
        }

        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _dbContext.Categories.FindAsync(dto.Id);

            if (category is null)
                return;

            category.Name = dto.Name;
            category.Slug = SlugHelper.GenerateSlug(dto.Slug);

            await _dbContext.SaveChangesAsync();
        }
    }
}
