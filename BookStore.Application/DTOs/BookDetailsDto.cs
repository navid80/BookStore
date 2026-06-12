namespace BookStore.Application.DTOs
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string Slug { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public string FilePath { get; set; } = null!;
        
        public int PageCount { get; set; }
        
        public string ISBN { get; set; } = null!;
        
        public int PublishedYear { get; set; }
        
        public string? CoverImagePath { get; set; }
        
        public bool IsPublished { get; set; }
        
        public bool IsFree { get; set; }

        public int CategoryId { get; set; }
        
        public string CategoryName { get; set; } = null!;
        
        public string CategorySlug { get; set; } = null!;

        public List<AuthorDetailsDto> Authors { get; set; } = new();
        
        public List<KeywordDetailsDto> Keywords { get; set; } = new();
        
        public List<BookListItemDto> RelatedBooks { get; set; } = new();
    }
}
