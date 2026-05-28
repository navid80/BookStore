namespace BookStore.Domain.Entities
{
    public class BookKeyword
    {
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;

        public int KeywordId { get; set; }

        public Keyword Keyword { get; set; } = null!;
    }
}
