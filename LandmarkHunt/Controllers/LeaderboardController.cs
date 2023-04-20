using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Constants;
using LandmarkHunt.Data;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace LandmarkHunt.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public LeaderboardController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult IndexAsync()
        {
            var model = new LeaderboardViewModel()
            {
                UserScores = _context.UserScores.OrderByDescending(x=>x.totalScore).Take(10).ToList()
            };
            if (User.Identity!.IsAuthenticated)
            {
                model.userPlace = _context.UserScores.OrderByDescending(x => x.totalScore).ToList().IndexOf(_context.UserScores.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)))+1;
                if (model.userPlace > 10)
                {
                    model.UserScores.Add(_context.UserScores.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)));
                }
            }

            return View(model);
        }
        public static async Task UpdateUserScoreAsync(string id, UserManager<AppUser> _userManager,AppDbContext _context) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return;
            }

            List<SessionChallenge> sessions = _context.SessionChallenges.Where(x => x.PlayerId == id).ToList();
            int score = sessions.Select(x => x.TotalScore).Sum();
            user.TotalScore = score;
            await _userManager.UpdateAsync(user);
            UserScore userScore = new UserScore();
            userScore.User = user;
            userScore.UserId = user.Id;
            userScore.totalScore = score;
            userScore.UserEmail = user.Email;
            if (_context.UserScores.Count(x => x.UserId == user.Id)==0)
            {
                _context.UserScores.Add(userScore);
            }
            else
            {
                _context.UserScores.Update(userScore);
            }
            _context.SaveChanges();
            return;
        }
        public static async Task InitialLeaderboard(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<AppUser>>();
            var context = service.GetService<AppDbContext>();
            var users = await userManager!.Users.ToListAsync();
            foreach (var user in users)
            {
                await UpdateUserScoreAsync(user.Id, userManager, context!);
            }
        }
        public static async Task UpdateAll(UserManager<AppUser> _userManager, AppDbContext _context)
        {
            var users = await _userManager!.Users.ToListAsync();
            foreach (var user in users)
            {
                await UpdateUserScoreAsync(user.Id, _userManager, _context!);
            }
        }
    }
}
