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
using Stripe;
using WA_StrayDogHelper_v1.Data;
using WA_StrayDogHelper_v1.Models.DomainModels;

namespace WA_StrayDogHelper_v1.Controllers
{
    public class RequestDonationMoneyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RequestDonationMoneyController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: RequestDonationMoney
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RequestDonationMoney.Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RequestDonationMoney/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationMoney = await _context.RequestDonationMoney
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestDonationMoney == null)
            {
                return NotFound();
            }

            return View(requestDonationMoney);
        }

        // GET: RequestDonationMoney/Create
        [Authorize]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: RequestDonationMoney/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Category,ProblemTitle,ProblemDescription,AmountOfMoneyRequired,DogName,ImageFile,EndDate,AmountOfDonatedMoney,UserId")] RequestDonationMoney request)
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

        [Authorize]
        // GET: RequestDonationMoney/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationMoney = await _context.RequestDonationMoney.FindAsync(id);
            if (requestDonationMoney == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/DonationImages", requestDonationMoney.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            return View(requestDonationMoney);
        }

        // POST: RequestDonationMoney/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category,ProblemTitle,ProblemDescription,AmountOfMoneyRequired,DogName,ImageFile,EndDate,AmountOfDonatedMoney,UserId")] RequestDonationMoney request)
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
                    if (!RequestDonationMoneyExists(request.Id))
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

        [Authorize]
        // GET: RequestDonationMoney/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestDonationMoney = await _context.RequestDonationMoney
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestDonationMoney == null)
            {
                return NotFound();
            }

            return View(requestDonationMoney);
        }

        [Authorize]
        // POST: RequestDonationMoney/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestDonationMoney = await _context.RequestDonationMoney.FindAsync(id);
            _context.RequestDonationMoney.Remove(requestDonationMoney);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestDonationMoneyExists(int id)
        {
            return _context.RequestDonationMoney.Any(e => e.Id == id);
        }

   

        

        [Authorize]
        public IActionResult DonirajVtoro(int id)
        {
            ViewBag.Broj = id;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DonirajVtoro([Bind("AmountOfMoney")] MakeDonationMoney makeDonation, int id)
        {
            if (ModelState.IsValid)
            {


                makeDonation.RequestDonationId = id;
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                makeDonation.UserId = loggedInUserId;


                _context.Add(makeDonation);
                await _context.SaveChangesAsync();
                var pisi = makeDonation.Id;
                return RedirectToAction("DonirajTreto", new { id = pisi });
            }
            return View(makeDonation);
        }

        [Authorize]
        public async Task<IActionResult> DonirajTreto(int id)
        {
            var napraviDonacija = await _context.MakeDonationMoney
               .FirstOrDefaultAsync(m => m.Id == id);

            return View(napraviDonacija);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DonirajTreto(string stripeEmail, string stripeToken, int id)
        {

            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var order = this._shoppingCartService.getShoppingCartInfo(userId);
            var napraviDonacija = _context.MakeDonationMoney.Find(id);

            var requestDonation = _context.RequestDonationMoney.Find(napraviDonacija.RequestDonationId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(napraviDonacija.AmountOfMoney) * 100),
                Description = "Donation for"+requestDonation.ProblemTitle,
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                //var result = this.Order();
                requestDonation.AmountOfDonatedMoney += napraviDonacija.AmountOfMoney;
                _context.RequestDonationMoney.Update(requestDonation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }

    }
}
