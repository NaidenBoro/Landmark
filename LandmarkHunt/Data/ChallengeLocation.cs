
namespace LandmarkHunt.Data;

public class ChallengeLocation
{

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ChallengeId { get; set; } = string.Empty;
    public Challenge Challenge { get; set; } = null!;

    public string LocationId { get; set; } = string.Empty;
    public Location Location { get; set; } = null!;
}
