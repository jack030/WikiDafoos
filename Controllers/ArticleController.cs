using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiDafoos.Models;
using WikiDafoos.Models.ViewModel;

using AutoMapper;
namespace WikiDafoos.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DafoosDbContext _dafoosDbContext;
        private readonly IMapper _mapper;

        public ArticleController(DafoosDbContext dafoosDbContext, IMapper mapper)
        {
            _dafoosDbContext = dafoosDbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dafoosDbContext.Articles.Include(a => a.Category).ToListAsync();
            if (model.Count == 0)
            {
                model = new List<Article>
                {
                    new Article
                    {
                        Content = "",
                        CreationTime = DateTime.Now,
                        Id = 22,
                        Title = "Hello",
                        Category = null,
                        IsPrivate = false,
                        Tags = new List<Tag>(),
                        IsDeleted = false,
                        Views = 0,
                        IsSuggested = false



                    }
                };

            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleViewModel creadteModel)
        {
            if (ModelState.IsValid)
            {
                var article = _mapper.Map<Article>(creadteModel);

                if (!string.IsNullOrEmpty(creadteModel.TagsInput))
                {
                    article.Tags = creadteModel.TagsInput
                        .Split(',')
                        .Select(tag => new Tag { Name = tag.Trim() })
                        .ToList();
                }

                if (creadteModel.CategoryId.HasValue)
                {
                    article.Category = await _dafoosDbContext.Categories.FindAsync(creadteModel.CategoryId.Value);
                }

                _dafoosDbContext.Articles.Add(article);
                await _dafoosDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _dafoosDbContext.Categories.ToListAsync();
            return View(creadteModel);
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

        [HttpGet]
        public async Task<IActionResult> SearchArticles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new List<object>());

            var results = await _dafoosDbContext.Articles
                .Where(a => a.Title.Contains(query) || a.Content.Contains(query))
                .Select(a => new { a.Id, a.Title })
                .Take(10)
                .ToListAsync();

            return Json(results);
        }
    }
}
