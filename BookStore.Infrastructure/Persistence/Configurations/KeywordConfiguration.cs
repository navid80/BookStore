using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations
{
    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.ToTable("Keywords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Word)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
