using BookStore.Application.DTOs;

namespace BookStore.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorDetailsDto>> GetAllAsync();

        Task<AuthorDetailsDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateAuthorDto dto);

        Task UpdateAsync(UpdateAuthorDto dto);

        Task DeleteAsync(int id);

        Task<List<AuthorDetailsDto>> Filter(string search);
    }
}
