using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
