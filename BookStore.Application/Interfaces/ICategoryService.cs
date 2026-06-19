using BookStore.Application.DTOs;

namespace BookStore.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDetailsDto>> GetAllAsync();

        Task<CategoryDetailsDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateCategoryDto dto);

        Task UpdateAsync(UpdateCategoryDto dto);

        Task DeleteAsync(int id);

        Task<List<CategoryDetailsDto>> Filter(string search);
    }
}
