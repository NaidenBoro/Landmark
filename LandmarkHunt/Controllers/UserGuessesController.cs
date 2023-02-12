using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LandmarkHunt.Controllers
{
    [Authorize]
    public class UserGuessesController : Controller
    {
        private readonly AppDbContext _context;

        public UserGuessesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserGuesses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserGuesses.Include(u => u.Location).Include(u => u.User).Where(x => x.User.Email == User.FindFirstValue(ClaimTypes.Email));
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserGuesses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UserGuesses == null)
            {
                return NotFound();
            }

            var userGuess = await _context.UserGuesses
                .Include(u => u.Location)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userGuess == null)
            {
                return NotFound();
            }

            return View(userGuess);
        }

        // GET: UserGuesses/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserGuesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,LocationId,Year,Latitude,Longitude,Score")] UserGuess userGuess)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userGuess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", userGuess.LocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGuess.UserId);
            return View(userGuess);
        }

        // GET: UserGuesses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UserGuesses == null)
            {
                return NotFound();
            }

            var userGuess = await _context.UserGuesses.FindAsync(id);
            if (userGuess == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", userGuess.LocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGuess.UserId);
            return View(userGuess);
        }

        // POST: UserGuesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,LocationId,Year,Latitude,Longitude,Score")] UserGuess userGuess)
        {
            if (id != userGuess.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userGuess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserGuessExists(userGuess.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", userGuess.LocationId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userGuess.UserId);
            return View(userGuess);
        }

        // GET: UserGuesses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UserGuesses == null)
            {
                return NotFound();
            }

            var userGuess = await _context.UserGuesses
                .Include(u => u.Location)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userGuess == null)
            {
                return NotFound();
            }

            return View(userGuess);
        }

        // POST: UserGuesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserGuesses == null)
            {
                return Problem("Entity set 'AppDbContext.UserGuesses'  is null.");
            }
            var userGuess = await _context.UserGuesses.FindAsync(id);
            if (userGuess != null)
            {
                _context.UserGuesses.Remove(userGuess);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserGuessExists(string id)
        {
          return _context.UserGuesses.Any(e => e.Id == id);
        }
    }
}
