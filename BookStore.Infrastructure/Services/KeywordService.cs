using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Services
{
    public class KeywordService : IKeywordService
    {
        private readonly AppDbContext _dbContext;

        public KeywordService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreateKeywordDto dto)
        {
            var keyword = new Keyword
            {
                Word = dto.Word
            };

            _dbContext.Keywords.Add(keyword);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var keyword = await _dbContext.Keywords.FindAsync(id);

            if (keyword is null)
                return;

            _dbContext.Keywords.Remove(keyword);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<KeywordDetailsDto>> Filter(string search)
        {
            var filteredKeywords = await _dbContext.Keywords
                .Where(x => x.Word.Contains(search))
                .Select(x => new KeywordDetailsDto
                {
                    Id = x.Id,
                    Word = x.Word
                })
                .ToListAsync();

            return filteredKeywords;
        }

        public async Task<List<KeywordDetailsDto>> GetAllAsync()
        {
            var keywords = await _dbContext.Keywords
                .Select(x => new KeywordDetailsDto
                {
                    Id = x.Id,
                    Word = x.Word
                })
                .ToListAsync();

            return keywords;
        }

        public async Task<KeywordDetailsDto> GetByIdAsync(int id)
        {
            var keyword = await _dbContext.Keywords
                .FirstOrDefaultAsync(x => x.Id == id);

            if(keyword is null)
                return null;

            var keywordDto = new KeywordDetailsDto
            {
                Id = keyword.Id,
                Word = keyword.Word
            };

            return keywordDto;
        }

        public async Task UpdateAsync(UpdateKeywordDto dto)
        {
            var keyword = await _dbContext.Keywords.FindAsync(dto.Id);

            if (keyword is null)
                return;

            keyword.Word = dto.Word;

            await _dbContext.SaveChangesAsync();
        }
    }
}
