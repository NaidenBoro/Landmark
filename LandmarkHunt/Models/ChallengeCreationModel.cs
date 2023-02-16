using LandmarkHunt.Data;

namespace LandmarkHunt.Models
{
    public class ChallengeCreationModel
    {
        public string Name { get; set; } = string.Empty;
        public  ICollection<Location> GuessedLocations { get; set; } = new List<Location>();
    }
}
