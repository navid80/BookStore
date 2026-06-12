namespace BookStore.Application.DTOs
{
    public class BookFilterDto
    {
        public string? SearchTerm { get; set; }

        public string? CategorySlug { get; set; }
        
        public string? AuthorSlug { get; set; }
        
        public string? KeywordWord { get; set; }
        
        public int Page { get; set; } = 1;
        
        public int PageSize { get; set; } = 12;
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        
        public int TotalCount { get; set; }
        
        public int Page { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        public bool HasPreviousPage => Page > 1;
        
        public bool HasNextPage => Page < TotalPages;
    }
}
