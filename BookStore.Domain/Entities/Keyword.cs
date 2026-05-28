using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Keyword : BaseEntity
    {
        public string Word { get; set; } = null!;

        public ICollection<BookKeyword> BookKeywords { get; set; }
            = new List<BookKeyword>();
    }
}
