
using LandmarkHunt.Areas.Identity.Data;

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

}
