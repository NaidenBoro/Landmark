using System.Collections.Generic;
using LandmarkHunt.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LandmarkHunt.Models
{

    public class LeaderboardViewModel
    {


        private readonly UserManager<AppUser>? _userManager;
        public List<AppUser> Users { get; set; } = new List<AppUser>();

        public LeaderboardViewModel(UserManager<AppUser>? userManager)
        {
            _userManager = userManager;
        }
        public LeaderboardViewModel()
        {
        }
    }
}
