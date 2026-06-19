using BookStore.Application.Common.Helpers;
using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _dbContext;

        public AuthorService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreateAuthorDto dto)
        {
            var author = new Author
            {
                FullName = dto.FullName,
                Slug = SlugHelper.GenerateSlug(dto.Slug)
            };

            _dbContext.Authors.Add(author);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);

            if (author is null)
                return;

            _dbContext.Authors.Remove(author);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<AuthorDetailsDto>> Filter(string search)
        {
            var filteredAuthors = await _dbContext.Authors
                .Where(x => x.FullName.Contains(search))
                .Select(x => new AuthorDetailsDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Slug = x.Slug
                })
                .ToListAsync();

            return filteredAuthors;
        }

        public async Task<List<AuthorDetailsDto>> GetAllAsync()
        {
            var authors = await _dbContext.Authors
                .Select(x => new AuthorDetailsDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Slug = x.Slug
                })
                .ToListAsync();

            return authors;
        }

        public async Task<AuthorDetailsDto?> GetByIdAsync(int id)
        {
            var author = await _dbContext.Authors
                .FirstOrDefaultAsync(x => x.Id == id);

            if (author is null)
                return null;

            var authorDto = new AuthorDetailsDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Slug = author.Slug
            };

            return authorDto;
        }

        public async Task UpdateAsync(UpdateAuthorDto dto)
        {
            var author = await _dbContext.Authors.FindAsync(dto.Id);

            if (author is null)
                return;

            author.FullName = dto.FullName;
            author.Slug = SlugHelper.GenerateSlug(dto.Slug);

            await _dbContext.SaveChangesAsync();
        }
    }
}
