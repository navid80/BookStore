namespace BookStore.Application.DTOs
{
    public class CreateAuthorDto
    {
        public string FullName { get; set; } = null!;

        public string Slug { get; set; } = null!;
    }
}
