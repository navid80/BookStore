namespace BookStore.Application.DTOs
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;
    }
}
