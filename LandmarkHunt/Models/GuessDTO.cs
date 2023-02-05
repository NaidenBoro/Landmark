using LandmarkHunt.Data;

namespace LandmarkHunt.Models
{
    public class GuessDTO
    {
        public Location Actual;
        public Location Guess;
        public int Score;
        public double Distance;
        public GuessDTO(Location actual, Location guess,int score,double distance)
        {
            Actual = actual;
            Guess = guess;
            Score = score;
            Distance = distance;
        }
    }
}
