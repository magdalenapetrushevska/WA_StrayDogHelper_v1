using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_StrayDogHelper_v1.Data;
using WA_StrayDogHelper_v1.Models.DomainModels;

namespace WA_StrayDogHelper_v1.Controllers
{
    public class RequestDonationFoodController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RequestDonationFoodController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: RequestDonationFood
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RequestDonationFood.Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RequestDonationFood/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationFood = await _context.RequestDonationFood
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestDonationFood == null)
            {
                return NotFound();
            }

            return View(requestDonationFood);
        }

        // GET: RequestDonationFood/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: RequestDonationFood/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProblemTitle,ProblemDescription,FoodType,AmountOfFoodRequired,DogName,ImageFile,UserId")] RequestDonationFood request)
        {
            if (ModelState.IsValid)
            {


                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                request.UserId = loggedInUserId;
                request.User = loggedInUser;


                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(request.ImageFile.FileName);
                string extension = Path.GetExtension(request.ImageFile.FileName);
                request.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/DonationImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }



                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(request);
        }

        // GET: RequestDonationFood/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationFood = await _context.RequestDonationFood.FindAsync(id);
            if (requestDonationFood == null)
            {
                return NotFound();
            }


            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/DonationImages", requestDonationFood.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            return View(requestDonationFood);
        }

        // POST: RequestDonationFood/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProblemTitle,ProblemDescription,FoodType,AmountOfFoodRequired,DogName,ImageFile,UserId")] RequestDonationFood request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                request.UserId = loggedInUserId;
                request.User = loggedInUser;


                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(request.ImageFile.FileName);
                string extension = Path.GetExtension(request.ImageFile.FileName);
                request.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/DonationImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }


                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestDonationFoodExists(request.Id))
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
           
            return View(request);
        }

        // GET: RequestDonationFood/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationFood = await _context.RequestDonationFood
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestDonationFood == null)
            {
                return NotFound();
            }

            return View(requestDonationFood);
        }

        // POST: RequestDonationFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestDonationFood = await _context.RequestDonationFood.FindAsync(id);
            _context.RequestDonationFood.Remove(requestDonationFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestDonationFoodExists(int id)
        {
            return _context.RequestDonationFood.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ContactForDonation(string id)
        {

            var userToContact = await _context.Users.FirstOrDefaultAsync(m => m.Id.Equals(id));
            return View(userToContact);
        }
    }
}
