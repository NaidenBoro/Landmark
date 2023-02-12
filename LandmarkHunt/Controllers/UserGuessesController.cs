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
        private bool UserGuessExists(string id)
        {
          return _context.UserGuesses.Any(e => e.Id == id);
        }
    }
}
