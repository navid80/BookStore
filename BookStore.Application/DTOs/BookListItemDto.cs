namespace BookStore.Application.DTOs
{
    public class BookListItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string CategorySlug { get; set; } = null!;

        public int PublishedYear { get; set; }

        public int PageCount { get; set; }

        public bool IsFree { get; set; }

        public string? CoverImagePath { get; set; }

        public List<string> AuthorNames { get; set; } = new();
    }
}
