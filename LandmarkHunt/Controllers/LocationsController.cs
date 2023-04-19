using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Globalization;
using LandmarkHunt.Services;
using LandmarkHunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace LandmarkHunt.Controllers
{

    [Authorize(Roles ="Admin")]
    public class LocationsController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public LocationsController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Locations.ToListAsync());
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new Location();
                location.CreatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                location.CreatorUser = _context.Users.First(x=>x.Id == location.CreatorUserId);
                model.UpdateLocation(location);
                foreach (var file in Request.Form.Files)
                {
                    Photo img = new Photo();

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.Bytes = ms.ToArray();

                    ms.Close();
                    ms.Dispose();

                    _context.Photos.Add(img);
                    location.PhotoUrl = img.Id;
                    location.Photo = img;
                }
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            LocModel model = new LocModel();
            model.Latitude = location.Latitude.ToString();
            model.Longitude = location.Longitude.ToString();
            model.Name = location.Name;
            model.Year = location.Year;
            model.Id = id;
            model.PhotoUrl = location.PhotoUrl;


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocModel model)
        {
            if (ModelState.IsValid)
            {
                var location = await _context.Locations.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (location == null) 
                {
                    return NotFound();
                }

                location.Photo = _context.Photos.First(x => x.Id == location.PhotoUrl);
                model.UpdateLocation(location);
                foreach (var file in Request.Form.Files)
                {
                    Photo img = new Photo();

                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.Bytes = ms.ToArray();

                    ms.Close();
                    ms.Dispose();
                    if (location.Photo.Bytes != img.Bytes)
                    {
                        _context.Photos.Add(img);
                        location.PhotoUrl = img.Id;
                        location.Photo = img;
                    }
                }
                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
              
                
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'AppDbContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                Location.DeleteLocationAndChallenges(location, _context);
                await LeaderboardController.UpdateAll(_userManager,_context);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(string id)
        {
          return _context.Locations.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Play(string? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        public async Task<IActionResult> Guess(string? id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocModel model)
        {
            if (ModelState.IsValid)
            {
                var guessYear = model.Year;
                var guessLatitude = double.Parse(model.Latitude, CultureInfo.InvariantCulture);
                var guessLongitude = double.Parse(model.Longitude, CultureInfo.InvariantCulture);

                var loc = await _context.Locations.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (loc == null) 
                {
                    return NotFound();
                }
                int Score = ScoreCalculator.GetScore(loc.Year, loc.Latitude, loc.Longitude, guessYear, guessLatitude, guessLongitude, "Easy");
                //Console.WriteLine(User.FindFirstValue(ClaimTypes.Email));
                var userGuess = new UserGuess();
                userGuess.Year = guessYear;
                userGuess.Latitude = guessLatitude;
                userGuess.Longitude = guessLongitude;
                userGuess.Score = Score;
                userGuess.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                userGuess.Location = loc;
                userGuess.User = _context.Users.First(x => x.Id == userGuess.UserId);
                _context.UserGuesses.Add(userGuess);
                await _context.SaveChangesAsync();
                return View(
                    new GuessModel(
                        loc.Name,
                        loc.Year,
                        loc.Latitude,
                        loc.Longitude,
                        guessYear,
                        guessLatitude,
                        guessLongitude,
                        Score,
                        ScoreCalculator.DistanceTo(loc.Latitude, loc.Longitude, guessLatitude, guessLongitude)));
            }
            //implement 404 page
            return RedirectToAction(nameof(Index));
        }
    }
}
