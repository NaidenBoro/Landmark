using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LandmarkHunt.Data;
using LandmarkHunt.Models;

namespace LandmarkHunt.Controllers
{
    public class LocationsController : Controller
    {
        private readonly AppDbContext _context;

        public LocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
              return View(await _context.Locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocDTO dto)
        {
            Location location = dto.toLocation();
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocDTO dto)
        {
            Location location = dto.toLocation();
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool LocationExists(int id)
        {
          return _context.Locations.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Play(int? id)
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

        public async Task<IActionResult> Guess(int id, [Bind("Id,Name,Year,Latitude,Longitude,PhotoUrl")] LocDTO dto)
        {
            Location guess = dto.toLocation();
            if (id != guess.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Location? loc = await _context.Locations.FirstOrDefaultAsync(x=> guess.Id == x.Id);
                    if (loc == null) 
                    {
                        return NotFound();
                    }
                    double Distance = DistanceScore(loc.Latitude,loc.Longitude, guess.Latitude, guess.Longitude) + YearScore(guess.Year,loc.Year);
                    Console.WriteLine(Distance);
                    return Json(Distance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(guess);
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public static int YearScore(int guess,int actual,int hardness=0)
        {
            //Graph for different hardness levels - https://www.desmos.com/calculator/xcdd0u3lbd
            double multiplier;
            switch (hardness)
            {
                
                case 1: //medium
                    multiplier = 2;
                    break;
                case 2: //hard
                    multiplier = 3;
                    break;
                default: //easy
                    multiplier = 1;
                    break;
            }

            double modifier = ((double)(2500 - actual))/(10*multiplier);
            double score = Math.Exp(-0.5*(Math.Pow((guess-actual)/modifier,2)));
            return (int)(score * multiplier*500);
        }

        public static int DistanceScore(double lat1, double lon1, double lat2, double lon2, int hardness = 0)
        {
            //Graph for different hardness levels - https://www.desmos.com/calculator/2udfjvm1h5
            double multiplier;
            switch (hardness)
            {

                case 1: //medium
                    multiplier = 2;
                    break;
                case 2: //hard
                    multiplier = 3;
                    break;
                default: //easy
                    multiplier = 1;
                    break;
            }
            double distance = DistanceTo(lat1, lon1, lat2, lon1);
            double score = Math.Max(Math.Min((200.1 / multiplier-distance)/(200/multiplier),500),0);
            return (int)(score * multiplier * 500);
        }
    }
}
