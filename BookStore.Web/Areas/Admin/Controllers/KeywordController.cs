using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KeywordController : Controller
    {
        private readonly IKeywordService _keywordService;

        public KeywordController(IKeywordService keywordService)
        {
            _keywordService = keywordService;
        }


        public async Task<IActionResult> Index()
        {
            var keywords = await _keywordService.GetAllAsync();

            return View(keywords);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateKeywordDto dto)
        {
            if(!ModelState.IsValid)
                return View(dto);

            await _keywordService.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var keyword = await _keywordService.GetByIdAsync(id);

            if (keyword is null)
                return NotFound();

            var editingKeyword = new UpdateKeywordDto
            {
                Id = keyword.Id,
                Word = keyword.Word
            };

            return View(editingKeyword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateKeywordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _keywordService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _keywordService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
