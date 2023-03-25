using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data
{
    public class UserScore
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public virtual AppUser User { get; set; } = new AppUser();
        public int totalScore { get; set; }

    }
}
