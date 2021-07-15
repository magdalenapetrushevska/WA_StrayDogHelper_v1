using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_StrayDogHelper_v1.Data;
using WA_StrayDogHelper_v1.Models.DomainModels;
using WA_StrayDogHelper_v1.Models.IdentityModels;

namespace WA_StrayDogHelper_v1.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
       /* public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Messages.Include(m => m.Receiver).Include(m => m.Sender);
            return View(await applicationDbContext.ToListAsync());
        }*/

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create(string id)
        {
            ViewBag.ReceiverId = id;
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content")] Message message, string id)
        {
            if (ModelState.IsValid)
            {

                message.ReceiverId = id;
                var receiverUser = _context.Users.Find(id);
                message.Receiver = receiverUser;

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);

                
                message.SenderId = loggedInUserId;
                message.Sender = loggedInUser;


                _context.Add(message);
                await _context.SaveChangesAsync();

                return RedirectToAction("VidiRazgovor", new { id = id } );
            }

            return View(message);
        }


        // GET: Messages/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
           
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SenderId,Content,ReceiverId")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            return View(message);
        }*/

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Receiver)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }



        public async Task<IActionResult> VidiKontakti()
        {
            //ova e momentalno najavenot korisnik
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);


            //treba da se zemat site messages vo koi daden korisnik ima udel (t.e. vo koi ili e vo uloga na sender ili vo uloga an receiver)
            var allMessages = await _context.Messages.Include(m =>m.Sender).Include(m =>m.Receiver)
                .Where(m => m.SenderId.Equals(loggedInUserId) || m.ReceiverId.Equals(loggedInUserId)).ToListAsync();


            List<ApplicationUser> listOfContacts = new List<ApplicationUser>();

            // na nas ni trebaat da gi izdvoime onie ApplicationUsers 
           foreach(var mess in allMessages)
            {
                if(!mess.SenderId.Equals(loggedInUserId)  && !listOfContacts.Contains(mess.Sender))
                {
                   
                    listOfContacts.Add(mess.Sender);
                }
                else if(!mess.ReceiverId.Equals(loggedInUserId) && !listOfContacts.Contains(mess.Receiver))
                {
                    listOfContacts.Add(mess.Receiver);
                }
            }

            return View(listOfContacts);
        }




        public async Task<IActionResult> VidiRazgovor(string id)
        {
            ViewBag.KorId = id;
            //zemi gi site messages koi se napraveni pomegju logiraniot korisnik i onoj sto ti e praten kako parametar

            //ova e momentalno najavenot korisnik
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);


            //treba da se zemat site messages vo koi daden korisnik ima udel (t.e. vo koi ili e vo uloga na sender ili vo uloga an receiver)
            var allMessages = await _context.Messages.Include(m => m.Sender).Include(m => m.Receiver)
                .Where(m => m.SenderId.Equals(loggedInUserId) || m.ReceiverId.Equals(loggedInUserId)).ToListAsync();

            //List<ApplicationUser> listOfContacts = new List<ApplicationUser>();
            List<Message> listOfMessages = new List<Message>();
            //od site tie messages ni trebaat samo onie kaj koi senderot ili receiverot e korisnikot do dadeno id
            foreach (var mess in allMessages)
            {
                if (!mess.SenderId.Equals(loggedInUserId) && mess.SenderId.Equals(id))
                {
                    
                    listOfMessages.Add(mess);
                }
                else if (!mess.ReceiverId.Equals(loggedInUserId) && mess.ReceiverId.Equals(id))
                {
                   
                    listOfMessages.Add(mess);
                }
            }

            return View(listOfMessages);
        }















    }
}
