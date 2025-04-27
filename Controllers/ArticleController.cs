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
        private readonly IWebHostEnvironment _environment;

        public ArticleController(DafoosDbContext dafoosDbContext, IMapper mapper, IWebHostEnvironment environment)
        {
            _dafoosDbContext = dafoosDbContext;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dafoosDbContext.Articles.Include(a => a.Category).Include(x=>x.Tags).ToListAsync();
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


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (upload == null || upload.Length == 0)
            {
                return Json(new { error = new { message = "No file uploaded." } });
            }

            try
            {
                // Define the path to store images
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(stream);
                }

                // Return the URL of the uploaded image
                //var fileUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{fileName}";
                var fileUrl = $"/Uploads/{fileName}";

                var result = new JsonResult(new { url = fileUrl, uploaded = true })
                {
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                return result;
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Upload error: {ex.Message}");
                return Json(new { error = new { message = $"Server error: {ex.Message}" } });
            }
        }
        public async Task<IActionResult> Create()
        {

            ViewBag.Categories = await _dafoosDbContext.Categories.ToListAsync();
            var model = new CreateArticleViewModel();
            return View(model);
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
            var article = await _dafoosDbContext.Articles.Include(a => a.Category).Include(x=>x.Tags).FirstOrDefaultAsync(a => a.Id == id);
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
                .Select(a => new { a.Id, a.Title ,a.IsPrivate,a.Image})
                .Take(10)
                .ToListAsync();

            return Json(results);
        }
    }
}
