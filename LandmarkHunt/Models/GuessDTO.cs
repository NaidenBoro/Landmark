using LandmarkHunt.Data;

namespace LandmarkHunt.Models
{
    public class GuessDTO
    {
        public Location Actual;
        public Location Guess;
        public int Score;

        public GuessDTO(Location actual, Location guess,int score)
        {
            Actual = actual;
            Guess = guess;
            Score = score;
        }
    }
}
