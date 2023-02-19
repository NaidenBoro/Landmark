using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using System.Security.Claims;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Authorization;

namespace LandmarkHunt.Controllers
{
    [Authorize]
    public class ChallengesController : Controller
    {
        private readonly AppDbContext _context;

        public ChallengesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Challenges.Include(c => c.CreatorUser).Where(x => x.CreatorUser.Email == User.FindFirstValue(ClaimTypes.Email));
            return View(await appDbContext.ToListAsync());
        }

        // GET: Challenges/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Challenges == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenges
                .Include(c => c.CreatorUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // GET: Challenges/Create
        public IActionResult Create()
        {
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id");
            ChallengeCreationModel model = new ChallengeCreationModel();
            model.GuessedLocations = _context.UserGuesses.Where(x => x.User.Email == User.FindFirstValue(ClaimTypes.Email)).Select(x => x.Location).Distinct().ToList();
            return View(model);
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Challenge challenge,List<string> locations)
        {
            challenge.ChallengeLocations = new List<ChallengeLocation>();
            challenge.CreatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            challenge.CreatorUser = _context.Users.First(x => x.Id == challenge.CreatorUserId);
            if (ModelState.IsValid)
            {
                _context.Add(challenge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id", challenge.CreatorUserId);
            return View(challenge);
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Challenges == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id", challenge.CreatorUserId);
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name")] Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challenge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeExists(challenge.Id))
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
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id", challenge.CreatorUserId);
            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Challenges == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenges
                .Include(c => c.CreatorUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Challenges == null)
            {
                return Problem("Entity set 'AppDbContext.Challenges'  is null.");
            }
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge != null)
            {
                _context.Challenges.Remove(challenge);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeExists(string id)
        {
          return _context.Challenges.Any(e => e.Id == id);
        }
    }
}
