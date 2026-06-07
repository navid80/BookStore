namespace BookStore.Application.DTOs
{
    public class AuthorDetailsDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Slug { get; set; } = null!;
    }
}
