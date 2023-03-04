using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Constants;
using LandmarkHunt.Data;
using LandmarkHunt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                await UpdateUserScoreAsync(user.Id);
            }

            var model = new LeaderboardViewModel(_userManager)
            {
                Users = users
            };
            

            return View(model);
        }
        public async Task UpdateUserScoreAsync(string id) 
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
            return;
        }
    }
}
