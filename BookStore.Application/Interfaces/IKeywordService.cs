using BookStore.Application.DTOs;

namespace BookStore.Application.Interfaces
{
    public interface IKeywordService
    {
        Task<List<KeywordDetailsDto>> GetAllAsync();

        Task<KeywordDetailsDto> GetByIdAsync(int id);

        Task CreateAsync(CreateKeywordDto dto);

        Task UpdateAsync(UpdateKeywordDto dto);

        Task DeleteAsync(int id);
    }
}
