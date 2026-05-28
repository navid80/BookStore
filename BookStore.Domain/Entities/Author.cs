using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public ICollection<BookAuthor> BookAuthors { get; set; }
            = new List<BookAuthor>();
    }
}
