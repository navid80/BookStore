using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _authorService.GetAllAsync();

            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _authorService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if(author is null)
                return NotFound();

            var editingAuthor = new UpdateAuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Slug = author.Slug
            };

            return View(editingAuthor);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _authorService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
