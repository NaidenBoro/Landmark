﻿using System;
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
        public async Task<IActionResult> YourChallenges()
        {
            var appDbContext = _context.Challenges.Include(c => c.CreatorUser).Where(x => x.CreatorUser.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await appDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
            List<SessionChallenge> startedGames = _context.SessionChallenges.Where(x => x.PlayerId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            var appDbContext = _context.Challenges.Include(c => c.CreatorUser).Where(x => !startedGames.Select(c=>c.ChallengeId).Contains(x.Id) && x.CreatorUser.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await appDbContext.ToListAsync());
        }
        

        

        // GET: Challenges/Create
        public IActionResult Create()
        {
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id");
            ChallengeCreationModel model = new ChallengeCreationModel();
            if (User.IsInRole("Admin"))
            {
                model.GuessedLocations = _context.Locations.ToList();
            }
            else
            {
                model.GuessedLocations = _context.UserGuesses.Where(x => x.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(x => x.Location).Distinct().ToList();
            }
            return View(model);
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Challenge challenge,List<string> locs)
        {

            
            List<Location> locations = new List<Location>();
            foreach (var el in locs)
            {
                locations.Add(_context.Locations.First(x=>x.Id == el));
            }
            if (locations.Count != 5)
            {
                //todo
                return BadRequest("You need 5 locations");
            }
            challenge.ChallengeLocations = new List<ChallengeLocation>();
            challenge.CreatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            challenge.CreatorUser = _context.Users.First(x => x.Id == challenge.CreatorUserId);
            if (ModelState.IsValid)
            {
                _context.Add(challenge);
                foreach(var el in locations)
                {
                    ChallengeLocation chl = new ChallengeLocation();
                    chl.Challenge = challenge;
                    chl.Location = el;
                    chl.ChallengeId = challenge.Id;
                    chl.LocationId = el.Id;
                    _context.Add(chl);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(YourChallenges));
            }
            ViewData["CreatorUserId"] = new SelectList(_context.Users, "Id", "Id", challenge.CreatorUserId);
            return View(challenge);
        }



        // GET: Challenges/Delete/5
        [Authorize(Roles = "Admin,Mod")]
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
        [Authorize(Roles = "Admin,Mod")]
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
            return RedirectToAction(nameof(YourChallenges));
        }

        private bool ChallengeExists(string id)
        {
          return _context.Challenges.Any(e => e.Id == id);
        }
    }
}
