using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private IBookService _bookService;
        private ICategoryService _categoryService;
        private IAuthorService _authorService;
        private IKeywordService _keywordService;

        public BookController(
            IBookService bookService,
            ICategoryService categoryService,
            IAuthorService authorService,
            IKeywordService keywordService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authorService = authorService;
            _keywordService = keywordService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View(new CreateBookDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(dto);
            }

            await _bookService.CreateAsync(dto);
            TempData["Success"] = "کتاب با موفقیت اضافه شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book is null)
                return NotFound();

            await PopulateDropdownsAsync(book);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBookDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(dto);
                return View(dto);
            }

            await _bookService.UpdateAsync(dto);
            TempData["Success"] = "کتاب با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            TempData["Success"] = "کتاب با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(dynamic? selected = null)
        {
            var categories = await _categoryService.GetAllAsync();
            var authors = await _authorService.GetAllAsync();
            var keywords = await _keywordService.GetAllAsync();
            var allBooks = await _bookService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name", selected?.CategoryId);
            ViewBag.Authors = authors;
            ViewBag.Keywords = keywords;
            ViewBag.AllBooks = allBooks;

            // برای چک‌باکس‌های انتخاب شده
            ViewBag.SelectedAuthorIds = selected?.AuthorIds ?? new List<int>();
            ViewBag.SelectedKeywordIds = selected?.KeywordIds ?? new List<int>();
            ViewBag.SelectedRelatedBookIds = selected?.RelatedBookIds ?? new List<int>();
        }

        public async Task<IActionResult> FilterBooks(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return RedirectToAction(nameof(Index));

            var filteredBooks = await _bookService.Filter(search);

            return View("Index", filteredBooks);
        }
    }
}
