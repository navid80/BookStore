using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IAuthorService _authorService;

        public BooksController(
            IBookService bookService,
            ICategoryService categoryService,
            IAuthorService authorService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authorService = authorService;
        }
        public async Task<IActionResult> Index(BookFilterDto filter)
        {
            var result = await _bookService.GetFilteredAsync(filter);
            var categories = await _categoryService.GetAllAsync();
            var authors = await _authorService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Authors = authors;
            ViewBag.Filter = filter;

            return View(result);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var book = await _bookService.GetBySlugAsync(slug);

            if (book is null)
                return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Download(string slug)
        {
            var book = await _bookService.GetBySlugAsync(slug);

            if (book is null)
                return NotFound();

            if (string.IsNullOrEmpty(book.FilePath))
                return NotFound();

            var physicalPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot",
                book.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(physicalPath))
                return NotFound();

            var fileName = $"{book.Title}.pdf";
            return PhysicalFile(physicalPath, "application/pdf", fileName);
        }
    }
}
