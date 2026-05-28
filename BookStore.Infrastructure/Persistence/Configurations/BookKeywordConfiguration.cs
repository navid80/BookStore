using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations
{
    public class BookKeywordConfiguration : IEntityTypeConfiguration<BookKeyword>
    {
        public void Configure(EntityTypeBuilder<BookKeyword> builder)
        {
            builder.ToTable("BookKeywords");

            builder.HasKey(x => new { x.BookId, x.KeywordId });

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BookKeywords)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Keyword)
                .WithMany(x => x.BookKeywords)
                .HasForeignKey(x => x.KeywordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
