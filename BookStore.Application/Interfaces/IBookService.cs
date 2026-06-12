using BookStore.Application.DTOs;

namespace BookStore.Application.Interfaces
{
    public interface IBookService
    {
        // Public
        //Task<PagedResult<BookListItemDto>> GetAllAsync(BookFilterDto filter);

        //Task<BookDetailsDto?> GetBySlugAsync(string slug);

        //Task<List<BookListItemDto>> GetByAuthorSlugAsync(string authorSlug);

        // Admin
        Task<List<BookListItemDto>> GetAllAsync();

        Task<UpdateBookDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateBookDto dto);

        Task UpdateAsync(UpdateBookDto dto);

        Task DeleteAsync(int id);
    }
}
