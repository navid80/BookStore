using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Author> Authors => Set<Author>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Keyword> Keywords => Set<Keyword>();

        public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();

        public DbSet<BookKeyword> BookKeywords => Set<BookKeyword>();

        public DbSet<BookRelation> BookRelations => Set<BookRelation>();
    }
}
