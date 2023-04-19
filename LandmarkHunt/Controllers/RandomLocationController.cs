using LandmarkHunt.Data;
using LandmarkHunt.Models;
using LandmarkHunt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace LandmarkHunt.Controllers
{
    [Authorize]
    public class RandomLocationController : Controller
    {

        private readonly AppDbContext _context;

        public RandomLocationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Play()
        {
            var locations = _context.Locations.ToList();
            if (locations.Count == 0)
            {
                return View("NoLocations");
            }

            var curLoc = locations[new Random().Next(locations.Count)];

            var img = _context.Photos.First(x => x.Id == curLoc.PhotoUrl);
            var imageBase64Data = Convert.ToBase64String(img.Bytes);
            var imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;
            return View("PlayRandomLocation", curLoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guess(string? id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocModel model)
        {
            if (ModelState.IsValid)
            {
                var guessYear = model.Year;
                var guessLatitude = double.Parse(model.Latitude, CultureInfo.InvariantCulture);
                var guessLongitude = double.Parse(model.Longitude, CultureInfo.InvariantCulture);

                var loc = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
                if (loc == null)
                {
                    return NotFound();
                }
                var score = ScoreCalculator.GetScore(loc.Year, loc.Latitude, loc.Longitude, guessYear, guessLatitude, guessLongitude, "Easy");
                var userGuess = new UserGuess
                {
                    Year = guessYear,
                    Latitude = guessLatitude,
                    Longitude = guessLongitude,
                    Score = score,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Location = loc,
                    Hardness = "Easy"
                };
                userGuess.User = _context.Users.First(x => x.Id == userGuess.UserId);
                _context.UserGuesses.Add(userGuess);
                
                return View(
                    "GuessRandomLocation",
                    new GuessModel(
                        loc.Name,
                        loc.Year,
                        loc.Latitude,
                        loc.Longitude,
                        guessYear,
                        guessLatitude,
                        guessLongitude,
                        score,
                        ScoreCalculator.DistanceTo(loc.Latitude, loc.Longitude, guessLatitude, guessLongitude)));
            }
            //implement 404 page
            return RedirectToAction(nameof(Index));
        }
    }
}
