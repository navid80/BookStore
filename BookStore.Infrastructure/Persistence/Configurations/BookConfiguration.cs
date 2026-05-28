using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(350);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.CoverImagePath)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(4000);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
