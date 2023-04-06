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
    public virtual Photo Photo { get; set;} = new Photo();

    public string CreatorUserId { get; set; } = string.Empty;
    public virtual AppUser CreatorUser { get; set; } = new AppUser();

    public virtual ICollection<UserGuess> UserGuesses { get; set; } = new List<UserGuess>();

    public virtual ICollection<ChallengeLocation> ChallengeLocations { get; set; } = new List<ChallengeLocation>();

    public static void DeleteLocationAndChallenges(Location location, AppDbContext context)
    {
        // delete all challenges that have this location
        var challengeLocations = context.ChallengeLocations.Where(cl => cl.LocationId == location.Id).ToList();

        // loop through each challenge location and remove its challenge
        foreach (var challengeLocation in challengeLocations)
        {
            var challenge = context.Challenges.First(x=>challengeLocation.ChallengeId==x.Id);
            Challenge.DeleteChallengeAndSessions(challenge,context);
        }

        // delete the location
        context.Locations.Remove(location);

        // save changes to the database
        context.SaveChanges();
    }
}
