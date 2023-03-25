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
using System.Globalization;
using LandmarkHunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace LandmarkHunt.Controllers
{
    public class SessionChallengesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SessionChallengesController(UserManager<AppUser> userManager, AppDbContext context)
        {

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.SessionChallenges.Include(s => s.Challenge).Include(s => s.Player).Where(x => x.PlayerId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await appDbContext.ToListAsync());
        }
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

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string id)
        {
            SessionChallenge sessionChallenge = new SessionChallenge();
            sessionChallenge.ChallengeId = id;
            sessionChallenge.Challenge = _context.Challenges.First(x => x.Id == id);
            sessionChallenge.Challenge.ChallengeLocations = _context.ChallengeLocations.Where(x => x.ChallengeId == sessionChallenge.ChallengeId).ToList();
            sessionChallenge.Challenge.Locations = sessionChallenge.Challenge.ChallengeLocations.Select(x => _context.Locations.First(y => y.Id == x.LocationId)).ToList();

            sessionChallenge.PlayerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            sessionChallenge.Player = _context.Users.First(x => x.Id == sessionChallenge.PlayerId);
            sessionChallenge.Progress = 0;
            if (ModelState.IsValid)
            {
                _context.Add(sessionChallenge);
                _context.SaveChanges();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", sessionChallenge.ChallengeId);
            ViewData["PlayerId"] = new SelectList(_context.Users, "Id", "Id", sessionChallenge.PlayerId);
            return Play(sessionChallenge.Id);
        }
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
        public IActionResult Play(string sessionId)
        {
            SessionChallenge sessionChallenge = getSession(sessionId);
            if (sessionChallenge.Progress >= 5)
            {
                return View("EndGame",sessionChallenge);
            }
            Location curLoc = sessionChallenge.Challenge.Locations
                .OrderBy(x => x.Id)
                .ToArray()[sessionChallenge.Progress];

            List<Location> guessed = _context.UserGuesses
                .Include(u => u.Location).Include(u => u.User)
                .Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(x => x.Location).ToList();
            if (guessed.Contains(curLoc))
            {
                //implement logic
                //return View("AlreadyGuessed");
                Console.WriteLine("here");
            }
            ViewData["sessionId"] = sessionId;
            Photo img = _context.Photos.First(x => x.Id == curLoc.PhotoUrl);
            string imageBase64Data = Convert.ToBase64String(img.Bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;
            return View("PLayLocation", curLoc);
        }
        private SessionChallenge getSession(string sessionId)
        {
            SessionChallenge sessionChallenge = _context.SessionChallenges.First(x => x.Id == sessionId);
            sessionChallenge.Challenge = _context.Challenges
                .First(x => x.Id == sessionChallenge.ChallengeId);
            sessionChallenge.Challenge.ChallengeLocations = _context.ChallengeLocations
                .Where(x => x.ChallengeId == sessionChallenge.ChallengeId)
                .ToList();
            sessionChallenge.Challenge.Locations = sessionChallenge.Challenge.ChallengeLocations
                .Select(x => _context.Locations
                .First(y => y.Id == x.LocationId))
                .ToList();
            var CurPlayerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurPlayerId != sessionChallenge.PlayerId)
            {
                //todo error
            }
            sessionChallenge.Player = _context.Users.First(x => x.Id == sessionChallenge.PlayerId);
            return sessionChallenge;
        }
        public async Task<IActionResult> Guess(string? id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocModel dto,string sessionId)
        {
            if (ModelState.IsValid)
            {
                var guessYear = dto.Year;
                var guessLatitude = double.Parse(dto.Latitude, CultureInfo.InvariantCulture);
                var guessLongitude = double.Parse(dto.Longitude, CultureInfo.InvariantCulture);

                var loc = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id);
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
                SessionChallenge session = getSession(sessionId);
                session.Progress++;
                session.TotalScore += Score;
                _context.SessionChallenges.Update(session);
                await LeaderboardController.UpdateUserScoreAsync(userGuess.UserId,_userManager, _context);
                await _context.SaveChangesAsync();
                ViewData["sessionId"] = sessionId;
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
        => DistanceScore(locLatitude, locLongitude, guessLatitude, guessLongitude, hardness) + YearScore(guessYear, locYear, hardness);
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
        public static int YearScore(int guess, int actual, int hardness)
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
            double modifier = ((double)(2500 - actual)) / (10 * multiplier);
            double score = Math.Exp(-0.5 * (Math.Pow((guess - actual) / modifier, 2)));
            return (int)(score * multiplier * 500);
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
        public string RetrieveImage(string Id)
        {
            Photo img = _context.Photos.First(x => x.Id == Id);
            string imageBase64Data = Convert.ToBase64String(img.Bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;
            return imageDataURL;
            //return View("Index");
        }
    }
}
