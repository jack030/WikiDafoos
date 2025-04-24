using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiDafoos.Models;

namespace WikiDafoos.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DafoosDbContext _dafoosDbContext;

        public ArticleController(DafoosDbContext dafoosDbContext)
        {
            _dafoosDbContext = dafoosDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _dafoosDbContext.Articles.Include(a => a.Category).ToListAsync();
            return View(articles);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                _dafoosDbContext.Articles.Add(article);
                await _dafoosDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var article = await _dafoosDbContext.Articles.FindAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                _dafoosDbContext.Update(article);
                await _dafoosDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _dafoosDbContext.Articles.FindAsync(id);
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _dafoosDbContext.Articles.FindAsync(id);
            _dafoosDbContext.Articles.Remove(article);
            await _dafoosDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _dafoosDbContext.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }
    }
}
