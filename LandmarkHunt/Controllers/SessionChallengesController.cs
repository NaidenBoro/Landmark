using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using System.Security.Claims;

namespace LandmarkHunt.Controllers
{
    public class SessionChallengesController : Controller
    {
        private readonly AppDbContext _context;

        public SessionChallengesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SessionChallenges
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SessionChallenges.Include(s => s.Challenge).Include(s => s.Player);
            return View(await appDbContext.ToListAsync());
        }

        // GET: SessionChallenges/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SessionChallenges == null)
            {
                return NotFound();
            }

            var sessionChallenge = await _context.SessionChallenges
                .Include(s => s.Challenge)
                .Include(s => s.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionChallenge == null)
            {
                return NotFound();
            }

            return View(sessionChallenge);
        }

        // GET: SessionChallenges/Create
        public IActionResult Create()
        {
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id");
            ViewData["PlayerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SessionChallenges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string challengeId)
        {
            SessionChallenge sessionChallenge = new SessionChallenge();
            sessionChallenge.ChallengeId = challengeId;
            sessionChallenge.Challenge = _context.Challenges.First(x => x.Id == challengeId);
            sessionChallenge.Challenge.ChallengeLocations = _context.ChallengeLocations.Where(x => x.ChallengeId == sessionChallenge.ChallengeId).ToList();
            sessionChallenge.Challenge.Locations = sessionChallenge.Challenge.ChallengeLocations.Select(x => _context.Locations.First(y => y.Id == x.LocationId)).ToList();

            sessionChallenge.PlayerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            sessionChallenge.Player = _context.Users.First(x => x.Id == sessionChallenge.PlayerId);
            sessionChallenge.Progress = 0;
            if (ModelState.IsValid)
            {
                _context.Add(sessionChallenge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", sessionChallenge.ChallengeId);
            ViewData["PlayerId"] = new SelectList(_context.Users, "Id", "Id", sessionChallenge.PlayerId);
            return View(sessionChallenge);
        }

        // GET: SessionChallenges/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SessionChallenges == null)
            {
                return NotFound();
            }

            var sessionChallenge = await _context.SessionChallenges.FindAsync(id);
            if (sessionChallenge == null)
            {
                return NotFound();
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", sessionChallenge.ChallengeId);
            ViewData["PlayerId"] = new SelectList(_context.Users, "Id", "Id", sessionChallenge.PlayerId);
            return View(sessionChallenge);
        }

        // POST: SessionChallenges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ChallengeId,PlayerId")] SessionChallenge sessionChallenge)
        {
            if (id != sessionChallenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessionChallenge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionChallengeExists(sessionChallenge.Id))
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
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", sessionChallenge.ChallengeId);
            ViewData["PlayerId"] = new SelectList(_context.Users, "Id", "Id", sessionChallenge.PlayerId);
            return View(sessionChallenge);
        }

        // GET: SessionChallenges/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SessionChallenges == null)
            {
                return NotFound();
            }

            var sessionChallenge = await _context.SessionChallenges
                .Include(s => s.Challenge)
                .Include(s => s.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionChallenge == null)
            {
                return NotFound();
            }

            return View(sessionChallenge);
        }

        // POST: SessionChallenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SessionChallenges == null)
            {
                return Problem("Entity set 'AppDbContext.SessionChallenges'  is null.");
            }
            var sessionChallenge = await _context.SessionChallenges.FindAsync(id);
            if (sessionChallenge != null)
            {
                _context.SessionChallenges.Remove(sessionChallenge);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionChallengeExists(string id)
        {
          return _context.SessionChallenges.Any(e => e.Id == id);
        }
    }
}
