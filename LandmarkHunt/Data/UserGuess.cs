using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data;

public class UserGuess
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = new AppUser();

    public int LocationId { get; set; }
    public virtual Location Location { get; set; } = new Location();

    public int Year { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Score { get; set; }
}
