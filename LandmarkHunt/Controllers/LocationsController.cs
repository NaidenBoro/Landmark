using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Globalization;

namespace LandmarkHunt.Controllers
{

    [Authorize(Roles ="Admin")]
    public class LocationsController : Controller
    {
        
        private readonly AppDbContext _context;

        public LocationsController(AppDbContext context)
        {
            _context = context;
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
            return View(location);
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

                model.UpdateLocation(location);

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
                _context.Locations.Remove(location);
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
                int Score = GetScore(loc.Year, loc.Latitude, loc.Longitude, guessYear, guessLatitude, guessLongitude);
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
                        DistanceTo(loc.Latitude, loc.Longitude, guessLatitude, guessLongitude)));
            }
            //implement 404 page
            return RedirectToAction(nameof(Index));
        }
        private static int GetScore(int locYear, double locLatitude, double locLongitude, int guessYear, double guessLatitude, double guessLongitude, int hardness = 0)
        =>  DistanceScore(locLatitude, locLongitude, guessLatitude, guessLongitude, hardness) + YearScore(guessYear, locYear, hardness);

        public static double DistanceTo(double locLatitude, double locLongitude, double guessLatitude, double guessLongitude, char unit = 'K')
        {
            double rlat1 = Math.PI * locLatitude / 180;
            double rlat2 = Math.PI * guessLatitude / 180;
            double theta = locLongitude - guessLongitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unit switch
            {
                //Kilometers -> default
                'K' => dist * 1.609344,
                //Nautical Miles 
                'N' => dist * 0.8684,
                //Miles
                'M' => dist,
                _ => dist,
            };
        }
        public static int YearScore(int guess,int actual,int hardness)
        {
            var multiplier = hardness switch
            {
                //medium
                1 => 2,
                //hard
                2 => 3,
                //easy
                _ => (double)1,
            };
            double modifier = ((double)(2500 - actual))/(10*multiplier);
            double score = Math.Exp(-0.5*(Math.Pow((guess-actual)/modifier,2)));
            return (int)(score * multiplier*500);
        }
        public static int DistanceScore(double locLatitude, double locLongitude, double guessLatitude, double guessLongitude, int hardness)
        {
            var multiplier = hardness switch
            {
                //medium
                1 => 2,
                //hard
                2 => 3,
                //easy
                _ => (double)1,
            };
            double distance = DistanceTo(locLatitude, locLongitude, guessLatitude, guessLongitude);
            double score = Math.Max(Math.Min((200.1 / multiplier - distance) / (200 / multiplier), 500), 0);
            return (int)(score * multiplier * 500);
        }
        
    }
}
