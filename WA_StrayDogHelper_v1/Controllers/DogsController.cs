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
    public class DogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public DogsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Dogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dogs.Include(d => d.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Dogs/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,AgeYears,AgeMonths,Sex,Size,Breed,ImageFile,Disabled,Sterilized,Vaccinated,Chipped,ShortLifeStory,Location,UserId")] Dog dog)
        {
            if (ModelState.IsValid)
            {

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                dog.UserId = loggedInUserId;
                dog.User = loggedInUser;


                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(dog.ImageFile.FileName);
                string extension = Path.GetExtension(dog.ImageFile.FileName);
                dog.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/AdoptionDogImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await dog.ImageFile.CopyToAsync(fileStream);
                }

                //insert record in database
                _context.Add(dog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dog.UserId);
            return View(dog);
        }

        [Authorize]
        // GET: Dogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.FindAsync(id);
            if (dog == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/AdoptionDogImages", dog.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            return View(dog);
        }

        // POST: Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AgeYears,AgeMonths,Sex,Size,Breed,ImageFile,Disabled,Sterilized,Vaccinated,Chipped,ShortLifeStory,Location,UserId")] Dog dog)
        {
            if (id != dog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                dog.UserId = loggedInUserId;
                dog.User = loggedInUser;

                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(dog.ImageFile.FileName);
                string extension = Path.GetExtension(dog.ImageFile.FileName);
                dog.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/AdoptionDogImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await dog.ImageFile.CopyToAsync(fileStream);
                }

                try
                {
                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.Id))
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
            
            return View(dog);
        }

        [Authorize]
        // GET: Dogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        [Authorize]
        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var toRemove = await _context.Dogs.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/AdoptionDogImages", toRemove.Name);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }



            var dog = await _context.Dogs.FindAsync(id);
            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.Id == id);
        }

        public IActionResult ListDogCategories()
        {
            return View();
        }


        public IActionResult ListAllDogs()
        {
            return RedirectToAction("Index");
        }

        public IActionResult ListSmallDogs()
        {

            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> smallDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.Size.ToString().Equals("Small"))
                {
                    smallDogs.Add(dog);
                }
            }

            return View(smallDogs);
        }

        public IActionResult ListMediumDogs()
        {
            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> mediumDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.Size.ToString().Equals("Medium"))
                {
                    mediumDogs.Add(dog);
                }
            }

            return View(mediumDogs);
        }

        public IActionResult ListLargeDogs()
        {
            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> largeDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.Size.ToString().Equals("Large"))
                {
                    largeDogs.Add(dog);
                }
            }

            return View(largeDogs);
        }

        public IActionResult ListBabyDogs()
        {
            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> babyDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.AgeYears == 0)
                {
                    babyDogs.Add(dog);
                }
            }

            return View(babyDogs);
        }



        public IActionResult ListAdultDogs()
        {
            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> adultDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.AgeYears > 0)
                {
                    adultDogs.Add(dog);
                }
            }

            return View(adultDogs);
        }
        public IActionResult ListDisabledDogs()
        {
            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> disabledDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {
                if (dog.Disabled.ToString().Equals("True"))
                {
                    disabledDogs.Add(dog);
                }
            }

            return View(disabledDogs);
        }

        [Authorize]
        public IActionResult DogsAddedByUser()
        {
            var allDogs = _context.Dogs.Include(d => d.User);

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var loggedInUser = _context.Users.Find(loggedInUserId);


            List<Dog> myDogs = new List<Dog>();

            foreach (Dog dog in allDogs)
            {
                if (dog.UserId.Equals(loggedInUserId))
                {
                    myDogs.Add(dog);
                }
            }

            return View(myDogs);
        }


        public IActionResult MaleDogs(string id)
        {

            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> maleDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {

                // za site kuchinija
                if (id == null || id.Equals(""))
                {
                    if (dog.Sex.ToString().Equals("Male"))
                    {

                        maleDogs.Add(dog);
                    }
                }

                //za disabled kucinja
                else if (id.Equals("Disabled"))
                {
                    if (dog.Disabled.ToString().Equals("True"))
                    {

                        if (dog.Sex.ToString().Equals("Male"))
                        {

                            maleDogs.Add(dog);
                        }
                    }
                }

                //id bazirano na size
                else if (id.Equals("Small") || id.Equals("Medium") || id.Equals("Large"))
                {
                    if (dog.Size.ToString().Equals(id))
                    {
                        if (dog.Sex.ToString().Equals("Male"))
                        {

                            maleDogs.Add(dog);
                        }
                    }
                }

                //spored Age
                else if (id.Equals("Baby") || id.Equals("Adult"))
                {

                    if (dog.AgeYears == 0)
                    {
                        if (id.Equals("Baby"))
                        {
                            if (dog.Sex.ToString().Equals("Male"))
                            {

                                maleDogs.Add(dog);
                            }
                        }
                    }
                    else if (dog.AgeYears > 0)
                    {
                        if (id.Equals("Adult"))
                        {
                            if (dog.Sex.ToString().Equals("Male"))
                            {

                                maleDogs.Add(dog);
                            }
                        }
                    }
                }


            }

            return View(maleDogs);
        }


        public IActionResult FemaleDogs(string id)
        {

            var allDogs = _context.Dogs.Include(d => d.User);
            List<Dog> femaleDogs = new List<Dog>();


            foreach (Dog dog in allDogs)
            {

                // za site kuchinija
                if (id == null || id.Equals(""))
                {
                    if (dog.Sex.ToString().Equals("Female"))
                    {

                        femaleDogs.Add(dog);
                    }
                }

                //za disabled kucinja
                else if (id.Equals("Disabled"))
                {
                    if (dog.Disabled.ToString().Equals("True"))
                    {

                        if (dog.Sex.ToString().Equals("Female"))
                        {

                            femaleDogs.Add(dog);
                        }
                    }
                }

                //id bazirano na size
                else if (id.Equals("Small") || id.Equals("Medium") || id.Equals("Large"))
                {
                    if (dog.Size.ToString().Equals(id))
                    {
                        if (dog.Sex.ToString().Equals("Female"))
                        {

                            femaleDogs.Add(dog);
                        }
                    }
                }

                //spored Age
                else if (id.Equals("Baby") || id.Equals("Adult"))
                {

                    if (dog.AgeYears == 0)
                    {
                        if (id.Equals("Baby"))
                        {
                            if (dog.Sex.ToString().Equals("Female"))
                            {

                                femaleDogs.Add(dog);
                            }
                        }
                    }
                    else if (dog.AgeYears > 0)
                    {
                        if (id.Equals("Adult"))
                        {
                            if (dog.Sex.ToString().Equals("Female"))
                            {

                                femaleDogs.Add(dog);
                            }
                        }
                    }
                }


            }

            return View(femaleDogs);
        }


        [Authorize]
        public async Task<IActionResult> AddToFavoriteDogs(int id)
        {
            FavoriteDog newFavoriteDog = new FavoriteDog();

            var favDog = _context.Dogs.Find(id);
            newFavoriteDog.DogId = id;
            newFavoriteDog.Dog = favDog;

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);
            newFavoriteDog.UserId = loggedInUserId;
            newFavoriteDog.User = loggedInUser;

            //sakam da proveram dali vo FavoriteDogs ima drugo kuce koi ima isto DogId I UserId kako ova novoto sto go pravime
            //ako e taka, nema da pravime nov zapis

            var siteFavorites = _context.FavoriteDogs;

            foreach(var fav in siteFavorites)
            {
                if((fav.DogId == newFavoriteDog.DogId && fav.UserId == newFavoriteDog.UserId))
                {
                    return RedirectToAction("UserFavoriteDogsList");
                }
            }

            _context.Add(newFavoriteDog);
            await _context.SaveChangesAsync();

            //loggedInUser.FavoriteDogs.Add(newFavoriteDog);

            return RedirectToAction("UserFavoriteDogsList");
        }

        [Authorize]
        public IActionResult UserFavoriteDogsList()
        {
            

            List<FavoriteDog> favoritesList = new List<FavoriteDog>();

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);

            var siteFavorites = _context.FavoriteDogs.Include(m =>m.Dog).Include(m => m.User).ThenInclude(m => m.FavoriteDogs);

            foreach (var fav in siteFavorites)
            {
                if (fav.UserId.Equals(loggedInUserId))
                {
                    favoritesList.Add(fav);
                }
            }


            return View(favoritesList);
        }



       [Authorize]
        public async Task<IActionResult> DeleteFromFavorites(int id)
        {

            var toRemove = await _context.FavoriteDogs.FindAsync(id);

           
            _context.FavoriteDogs.Remove(toRemove);
            await _context.SaveChangesAsync();

            /*var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);

            loggedInUser.FavoriteDogs.Remove(toRemove);
*/

            return RedirectToAction(nameof(UserFavoriteDogsList));
        }


         /*var requestDonationMoney = await _context.RequestDonationMoney
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);*/



        public async Task<IActionResult> ContactForAdoption(string id)
        {

            var userToContact = await _context.Users.FirstOrDefaultAsync(m => m.Id.Equals(id));
            return View(userToContact);
        }



    }
}
