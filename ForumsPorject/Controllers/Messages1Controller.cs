//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using ForumsPorject.Repository;
//using ForumsPorject.Repository.Entites;

//namespace ForumsProject.Controllers
//{
//    public class Messages1Controller : Controller
//    {
//        private readonly DB_ForumsDbContext _context;

//        public Messages1Controller(DB_ForumsDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Messages1
//        public async Task<IActionResult> Index()
//        {
//            var dB_ForumsDbContext = _context.Messages.Include(m => m.Auteur).Include(m => m.Discussion);
//            return View(await dB_ForumsDbContext.ToListAsync());
//        }

//        // GET: Messages1/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Messages == null)
//            {
//                return NotFound();
//            }

//            var message = await _context.Messages
//                .Include(m => m.Auteur)
//                .Include(m => m.Discussion)
//                .FirstOrDefaultAsync(m => m.MessagesId == id);
//            if (message == null)
//            {
//                return NotFound();
//            }

//            return View(message);
//        }

//        // GET: Messages1/Create
//        public IActionResult Create()
//        {
//            ViewData["AuteurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "UtilisateurId");
//            ViewData["Discussionid"] = new SelectList(_context.Discussions, "DiscussionId", "DiscussionId");
//            return View();
//        }

//        // POST: Messages1/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("MessagesId,ContenuMessage,DatecréationMessage,Lu,Archive,AuteurId,Discussionid")] Message message)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(message);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AuteurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "UtilisateurId", message.AuteurId);
//            ViewData["Discussionid"] = new SelectList(_context.Discussions, "DiscussionId", "DiscussionId", message.Discussionid);
//            return View(message);
//        }

//        // GET: Messages1/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Messages == null)
//            {
//                return NotFound();
//            }

//            var message = await _context.Messages.FindAsync(id);
//            if (message == null)
//            {
//                return NotFound();
//            }
//            ViewData["AuteurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "UtilisateurId", message.AuteurId);
//            ViewData["Discussionid"] = new SelectList(_context.Discussions, "DiscussionId", "DiscussionId", message.Discussionid);
//            return View(message);
//        }

//        // POST: Messages1/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("MessagesId,ContenuMessage,DatecréationMessage,Lu,Archive,AuteurId,Discussionid")] Message message)
//        {
//            if (id != message.MessagesId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(message);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!MessageExists(message.MessagesId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AuteurId"] = new SelectList(_context.Utilisateurs, "UtilisateurId", "UtilisateurId", message.AuteurId);
//            ViewData["Discussionid"] = new SelectList(_context.Discussions, "DiscussionId", "DiscussionId", message.Discussionid);
//            return View(message);
//        }

//        // GET: Messages1/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Messages == null)
//            {
//                return NotFound();
//            }

//            var message = await _context.Messages
//                .Include(m => m.Auteur)
//                .Include(m => m.Discussion)
//                .FirstOrDefaultAsync(m => m.MessagesId == id);
//            if (message == null)
//            {
//                return NotFound();
//            }

//            return View(message);
//        }

//        // POST: Messages1/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Messages == null)
//            {
//                return Problem("Entity set 'DB_ForumsDbContext.Messages'  is null.");
//            }
//            var message = await _context.Messages.FindAsync(id);
//            if (message != null)
//            {
//                _context.Messages.Remove(message);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool MessageExists(int id)
//        {
//          return (_context.Messages?.Any(e => e.MessagesId == id)).GetValueOrDefault();
//        }
//    }
//}
