using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public ICollection<Book> Books { get; set; }
            = new List<Book>();
    }
}
