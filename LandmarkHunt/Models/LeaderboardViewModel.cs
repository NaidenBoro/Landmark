using System.Collections.Generic;
using LandmarkHunt.Areas.Identity.Data;
using LandmarkHunt.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LandmarkHunt.Models
{

    public class LeaderboardViewModel
    {
        public List<UserScore> UserScores { get; set; } = new List<UserScore>();
        public int userPlace { get; set; } = 0;

        public LeaderboardViewModel()
        {
        }
    }
}
