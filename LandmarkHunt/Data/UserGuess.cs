using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data;

public class UserGuess
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = new AppUser();

    public string LocationId { get; set; } = string.Empty;
    public virtual Location Location { get; set; } = new Location();

    public string Hardness { get; set; } = "Easy";

    public int Year { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Score { get; set; }
}
