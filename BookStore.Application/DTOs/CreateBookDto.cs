using Microsoft.AspNetCore.Http;

namespace BookStore.Application.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = null!;
        
        public string Slug { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public int PageCount { get; set; }
        
        public string ISBN { get; set; } = null!;
        
        public int PublishedYear { get; set; }
        
        public bool IsPublished { get; set; }
        
        public bool IsFree { get; set; }
        
        public int CategoryId { get; set; }

        public IFormFile? PdfFile { get; set; }

        public IFormFile? CoverImage { get; set; }

        public List<int> AuthorIds { get; set; } = new();

        public List<int> KeywordIds { get; set; } = new();

        public List<int> RelatedBookIds { get; set; } = new();
    }
}
