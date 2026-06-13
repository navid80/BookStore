using BookStore.Application.DTOs;

namespace BookStore.Application.Interfaces
{
    public interface IBookService
    {
        // Public
        Task<PagedResult<BookListItemDto>> GetFilteredAsync(BookFilterDto filter);

        Task<BookDetailsDto?> GetBySlugAsync(string slug);

        // Admin
        Task<List<BookListItemDto>> GetAllAsync();

        Task<UpdateBookDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateBookDto dto);

        Task UpdateAsync(UpdateBookDto dto);

        Task DeleteAsync(int id);
    }
}
