using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Book : BaseEntity
    {
        /// <summary>
        /// عنوان کتاب
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// slug
        /// </summary>
        public string Slug { get; set; } = null!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// مسیر فایل pdf
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// تعداد صفحات کتاب
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// شابک - شماره استاندارد بین المللی کتاب
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// سال انتشار
        /// </summary>
        public int PublishedYear { get; set; }

        /// <summary>
        /// مسیر عکس جلد کتاب
        /// </summary>
        public string? CoverImagePath { get; set; }

        /// <summary>
        /// آیا منتشر شده است؟
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// آیا کتاب رایگان است؟
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// دسته بندی کتاب
        /// </summary>
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<BookKeyword> BookKeywords { get; set; } = new List<BookKeyword>();

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
            
        public ICollection<BookRelation> RelatedTo { get; set; } = new List<BookRelation>();

        public ICollection<BookRelation> RelatedFrom { get; set; } = new List<BookRelation>();
    }
}
