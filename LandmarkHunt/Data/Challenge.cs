
using LandmarkHunt.Areas.Identity.Data;
using Microsoft.CodeAnalysis;

namespace LandmarkHunt.Data;

public class Challenge
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;

    public string CreatorUserId { get; set; } = string.Empty;
    public virtual AppUser CreatorUser { get; set; } = new AppUser();

    public virtual ICollection<ChallengeLocation> ChallengeLocations { get; set; } = new List<ChallengeLocation>();
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    public virtual ICollection<SessionChallenge> Sessions { get; set; } = new List<SessionChallenge>();
    public static void DeleteChallengeAndSessions(Challenge challenge, AppDbContext context)
    {
        // delete all challenges that have this location
        var sessionChallenges = context.SessionChallenges.Where(sc => sc.ChallengeId == challenge.Id).ToList();
        var challengeLocations = context.ChallengeLocations.Where(cl => cl.ChallengeId == challenge.Id).ToList();

        // loop through each challenge location and remove its challenge
        foreach (var session in sessionChallenges)
        {
            context.SessionChallenges.Remove(session);
        }

        foreach (var challengeLocation in challengeLocations)
        {
            context.ChallengeLocations.Remove(challengeLocation);
        }

        // delete the location
        context.Challenges.Remove(challenge);

        // save changes to the database
        context.SaveChanges();
    }

}
