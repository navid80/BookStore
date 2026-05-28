namespace BookStore.Domain.Entities
{
    public class BookRelation
    {
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;

        public int RelatedBookId { get; set; }

        public Book RelatedBook { get; set; } = null!;
    }
}
