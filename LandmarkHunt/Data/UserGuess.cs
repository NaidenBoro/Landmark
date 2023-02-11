using LandmarkHunt.Areas.Identity.Data;

namespace LandmarkHunt.Data
{
    public class UserGuess
    {
        public AppUser User { get; set; }
        public Location Location { get; set; }
        public int Year { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Score { get; set; }

        public UserGuess(AppUser user, Location loc, int year, double lat, double lon, int score) 
        {
            User = user;
            Location = loc;
            Year = year;
            Latitude = lat;
            Longitude = lon;
            Score = score;
        }

    }
}
