using BookStore.Application.Common.Helpers;
using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _webRootPath;

        public BookService(AppDbContext dbContext, string webRootPath)
        {
            _dbContext = dbContext;
            _webRootPath = webRootPath;
        }

        public async Task<UpdateBookDto?> GetByIdAsync(int id)
        {
            var book = await _dbContext.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookKeywords)
                .Include(b => b.RelatedTo)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null) return null;

            var editingBook = new UpdateBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Slug = book.Slug,
                Description = book.Description,
                PageCount = book.PageCount,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                IsPublished = book.IsPublished,
                IsFree = book.IsFree,
                CategoryId = book.CategoryId,
                CurrentFilePath = book.FilePath,
                CurrentCoverImagePath = book.CoverImagePath,
                AuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList(),
                KeywordIds = book.BookKeywords.Select(bk => bk.KeywordId).ToList(),
                RelatedBookIds = book.RelatedTo.Select(br => br.RelatedBookId).ToList()
            };

            return editingBook;
        }

        public async Task CreateAsync(CreateBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Slug = SlugHelper.GenerateSlug(dto.Slug),
                Description = dto.Description,
                PageCount = dto.PageCount,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                IsFree = dto.IsFree,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                FilePath = await SaveFileAsync(dto.PdfFile, "pdfs") ?? string.Empty,
                CoverImagePath = await SaveFileAsync(dto.CoverImage, "covers")
            };

            foreach (var authorId in dto.AuthorIds)
                book.BookAuthors.Add(new BookAuthor { AuthorId = authorId });

            foreach(var keywordId in dto.KeywordIds)
                book.BookKeywords.Add(new BookKeyword { KeywordId = keywordId });

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            foreach(var relatedId in dto.RelatedBookIds)
                _dbContext.BookRelations.Add(new BookRelation { BookId = book.Id, RelatedBookId = relatedId });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book is null)
                return;

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BookListItemDto>> GetAllAsync()
        {
            var books = await _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookListItemDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Slug = b.Slug,
                    CategoryName = b.Category.Name,
                    CategorySlug = b.Category.Slug,
                    PublishedYear = b.PublishedYear,
                    PageCount = b.PageCount,
                    IsFree = b.IsFree,
                    CoverImagePath = b.CoverImagePath,
                    AuthorNames = b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                })
                .ToListAsync() ?? new List<BookListItemDto>();

            return books;
        }

        public async Task UpdateAsync(UpdateBookDto dto)
        {
            var book = await _dbContext.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookKeywords)
                .Include(b => b.RelatedTo)
                .FirstOrDefaultAsync(b => b.Id == dto.Id);

            if (book is null)
                return;

            book.Title = dto.Title;
            book.Slug = SlugHelper.GenerateSlug(dto.Slug);
            book.Description = dto.Description;
            book.PageCount = dto.PageCount;
            book.ISBN = dto.ISBN;
            book.PublishedYear = dto.PublishedYear;
            book.IsPublished = dto.IsPublished;
            book.IsFree = dto.IsFree;
            book.CategoryId = dto.CategoryId;
            book.ModifiedAt = DateTime.UtcNow;

            if (dto.PdfFile != null)
                book.FilePath = await SaveFileAsync(dto.PdfFile, "pdfs") ?? book.FilePath;

            if (dto.CoverImage != null)
                book.CoverImagePath = await SaveFileAsync(dto.CoverImage, "covers");

            _dbContext.BookAuthors.RemoveRange(book.BookAuthors);
            foreach (var authorId in dto.AuthorIds)
                book.BookAuthors.Add(new BookAuthor { BookId = book.Id, AuthorId = authorId });

            _dbContext.BookKeywords.RemoveRange(book.BookKeywords);
            foreach (var keywordId in dto.KeywordIds)
                book.BookKeywords.Add(new BookKeyword { BookId = book.Id, KeywordId = keywordId });

            _dbContext.BookRelations.RemoveRange(book.RelatedTo);
            foreach (var relatedId in dto.RelatedBookIds)
                _dbContext.BookRelations.Add(new BookRelation { BookId = book.Id, RelatedBookId = relatedId });

            await _dbContext.SaveChangesAsync();
        }

        private async Task<string> SaveFileAsync(IFormFile? file, string folder)
        {
            if (file is null || file.Length == 0)
                return null;

            var uploadPath = Path.Combine(_webRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }

        public async Task<PagedResult<BookListItemDto>> GetFilteredAsync(BookFilterDto filter)
        {
            var query = _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(b => b.BookKeywords).ThenInclude(bk => bk.Keyword)
                .Where(b => b.IsPublished)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    (b.Description != null && b.Description.ToLower().Contains(term)) ||
                    b.BookAuthors.Any(ba => ba.Author.FullName.ToLower().Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(filter.CategorySlug))
                query = query.Where(b => b.Category.Slug == filter.CategorySlug);

            if (!string.IsNullOrWhiteSpace(filter.AuthorSlug))
                query = query.Where(b => b.BookAuthors.Any(ba => ba.Author.Slug == filter.AuthorSlug));

            if (!string.IsNullOrWhiteSpace(filter.KeywordWord))
                query = query.Where(b => b.BookKeywords.Any(bk => bk.Keyword.Word == filter.KeywordWord));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(b => new BookListItemDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Slug = b.Slug,
                    CategoryName = b.Category.Name,
                    CategorySlug = b.Category.Slug,
                    PublishedYear = b.PublishedYear,
                    PageCount = b.PageCount,
                    IsFree = b.IsFree,
                    CoverImagePath = b.CoverImagePath,
                    AuthorNames = b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                })
                .ToListAsync();

            return new PagedResult<BookListItemDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<BookDetailsDto?> GetBySlugAsync(string slug)
        {
            var book = await _dbContext.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(b => b.BookKeywords).ThenInclude(bk => bk.Keyword)
                .Include(b => b.RelatedTo).ThenInclude(br => br.RelatedBook)
                    .ThenInclude(rb => rb.Category)
                .Include(b => b.RelatedTo).ThenInclude(br => br.RelatedBook)
                    .ThenInclude(rb => rb.BookAuthors).ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);

            if (book is null) return null;

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Slug = book.Slug,
                Description = book.Description,
                FilePath = book.FilePath,
                PageCount = book.PageCount,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                CoverImagePath = book.CoverImagePath,
                IsPublished = book.IsPublished,
                IsFree = book.IsFree,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
                CategorySlug = book.Category.Slug,
                Authors = book.BookAuthors.Select(ba => new AuthorDetailsDto
                {
                    Id = ba.Author.Id,
                    FullName = ba.Author.FullName,
                    Slug = ba.Author.Slug
                }).ToList(),
                Keywords = book.BookKeywords.Select(bk => new KeywordDetailsDto
                {
                    Id = bk.Keyword.Id,
                    Word = bk.Keyword.Word
                }).ToList(),
                RelatedBooks = book.RelatedTo.Select(br => new BookListItemDto
                {
                    Id = br.RelatedBook.Id,
                    Title = br.RelatedBook.Title,
                    Slug = br.RelatedBook.Slug,
                    CategoryName = br.RelatedBook.Category.Name,
                    CategorySlug = br.RelatedBook.Category.Slug,
                    PublishedYear = br.RelatedBook.PublishedYear,
                    PageCount = br.RelatedBook.PageCount,
                    IsFree = br.RelatedBook.IsFree,
                    CoverImagePath = br.RelatedBook.CoverImagePath,
                    AuthorNames = br.RelatedBook.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                }).ToList()
            };
        }

        public async Task<List<BookListItemDto>> Filter(string search)
        {
            var filteredBooks = await _dbContext.Books
                .Where(x => x.Title.Contains(search))
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookListItemDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Slug = b.Slug,
                    CategoryName = b.Category.Name,
                    CategorySlug = b.Category.Slug,
                    PublishedYear = b.PublishedYear,
                    PageCount = b.PageCount,
                    IsFree = b.IsFree,
                    CoverImagePath = b.CoverImagePath,
                    AuthorNames = b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
                })
                .ToListAsync() ?? new List<BookListItemDto>();

            return filteredBooks;
        }
    }
}
