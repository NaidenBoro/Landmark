
namespace LandmarkHunt.Data;

public class ChallengeLocation
{

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ChallengeId { get; set; } = string.Empty;
    public virtual Challenge Challenge { get; set; } = new Challenge();

    public string LocationId { get; set; } = string.Empty;
    public virtual Location Location { get; set; } = new Location();
}
