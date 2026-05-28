using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations
{
    public class BookRelationConfiguration : IEntityTypeConfiguration<BookRelation>
    {
        public void Configure(EntityTypeBuilder<BookRelation> builder)
        {
            builder.ToTable("BookRelations");

            builder.HasKey(x => new { x.BookId, x.RelatedBookId });

            builder.HasOne(x => x.Book)
                .WithMany(x => x.RelatedTo)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RelatedBook)
                .WithMany(x => x.RelatedFrom)
                .HasForeignKey(x => x.RelatedBookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
