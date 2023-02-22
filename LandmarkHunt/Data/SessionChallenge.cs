using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data
{
    public class SessionChallenge
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ChallengeId { get; set; } = string.Empty;
        public virtual Challenge Challenge { get; set; } = new Challenge();
        public string PlayerId { get; set; } = string.Empty;
        public virtual AppUser Player { get; set; } = new AppUser();

        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
        public int Progress;
    }
}
