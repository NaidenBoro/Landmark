using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data;

public class Location
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;

    public string CreatorUserId { get; set; } = string.Empty;
    public virtual AppUser CreatorUser { get; set; } = new AppUser();

    public virtual ICollection<UserGuess> UserGuesses { get; set; } = null!;

    //public virtual ICollection<ChallengeLocation> ChallengeLocations { get; set; } = null!;
}
