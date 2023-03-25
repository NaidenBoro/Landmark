using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandmarkHunt.Data;
using Microsoft.AspNetCore.Identity;

namespace LandmarkHunt.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    public virtual ICollection<UserGuess> UserGuesses { get; set; } = new List<UserGuess>();
    public virtual ICollection<SessionChallenge> Sessions { get; set; } = new List<SessionChallenge>();
    public int TotalScore;
}

