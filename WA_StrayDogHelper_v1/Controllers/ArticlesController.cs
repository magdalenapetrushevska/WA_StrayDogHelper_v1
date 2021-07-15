using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_StrayDogHelper_v1.Data;
using WA_StrayDogHelper_v1.Models.DomainModels;

namespace WA_StrayDogHelper_v1.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArticlesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Articles.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        // GET: Articles/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Created,Tag,TimeRequiredToRead,NumberOfApplause,ImageFile,UserId")] Article article)
        {
            if (ModelState.IsValid)
            {

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                article.UserId = loggedInUserId;
                article.User = loggedInUser;
                article.Created = DateTime.Now;

                //calculate minutes needed to read the article

                var l = 0;
                var numWords = 1;
               
                while (l <= article.Content.Length - 1)
                {      
                    if (article.Content[l] == ' ' || article.Content[l] == '\n' || article.Content[l] == '\t')
                    {
                        numWords++;
                    }
                    l++;
                }

                var timeToReadInMin = numWords / 200;
                if(timeToReadInMin == 0)
                {
                    article.TimeRequiredToRead = "less than a minute";
                }
                else if (timeToReadInMin == 1)
                {
                    article.TimeRequiredToRead = "1 minute";
                }
                else
                {
                    article.TimeRequiredToRead = timeToReadInMin.ToString() + " minutes";
                }


                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(article.ImageFile.FileName);
                string extension = Path.GetExtension(article.ImageFile.FileName);
                article.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/ImagesForArticles", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await article.ImageFile.CopyToAsync(fileStream);
                }


                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(article);
        }
        [Authorize]
        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/ImagesForArticles", article.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            return View(article);
        }

        [Authorize]
        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Created,Tag,TimeRequiredToRead,NumberOfApplause,ImageFile,UserId")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                article.UserId = loggedInUserId;
                article.User = loggedInUser;
                article.Created = DateTime.Now;

                //calculate minutes needed to read the article

                var l = 0;
                var numWords = 1;

                while (l <= article.Content.Length - 1)
                {
                    if (article.Content[l] == ' ' || article.Content[l] == '\n' || article.Content[l] == '\t')
                    {
                        numWords++;
                    }
                    l++;
                }

                var timeToReadInMin = numWords / 200;
                if (timeToReadInMin == 0)
                {
                    article.TimeRequiredToRead = "less than a minute";
                }
                else if (timeToReadInMin == 1)
                {
                    article.TimeRequiredToRead = "1 minute";
                }
                else
                {
                    article.TimeRequiredToRead = timeToReadInMin.ToString() + " minutes";
                }

                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(article.ImageFile.FileName);
                string extension = Path.GetExtension(article.ImageFile.FileName);
                article.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/ImagesForArticles", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await article.ImageFile.CopyToAsync(fileStream);
                }


                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(article);
        }

        [Authorize]
        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> AddToFavoriteArticles(int id)
        {
            FavoriteArticle newFavoriteArticle = new FavoriteArticle();

            var favArticle = _context.Articles.Find(id);
            newFavoriteArticle.ArticleId = id;
            newFavoriteArticle.Article = favArticle;

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);
            newFavoriteArticle.UserId = loggedInUserId;
            newFavoriteArticle.User = loggedInUser;

           
            var siteFavorites = _context.FavoriteArticles;

            foreach (var fav in siteFavorites)
            {
                if ((fav.ArticleId == newFavoriteArticle.ArticleId && fav.UserId == newFavoriteArticle.UserId))
                {
                    return RedirectToAction("UserFavoriteArticleList");
                }
            }

            _context.Add(newFavoriteArticle);
            await _context.SaveChangesAsync();

            //loggedInUser.FavoriteDogs.Add(newFavoriteDog);

            return RedirectToAction("UserFavoriteArticleList");
        }


        [Authorize]
        public IActionResult UserFavoriteArticleList()
        {


            List<FavoriteArticle> favoritesList = new List<FavoriteArticle>();

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);

            var siteFavorites = _context.FavoriteArticles.Include(m => m.Article).Include(m => m.User).ThenInclude(m => m.FavoriteArticles);

            foreach (var fav in siteFavorites)
            {
                if (fav.UserId.Equals(loggedInUserId))
                {
                    favoritesList.Add(fav);
                }
            }


            return View(favoritesList);
        }




    }
}
