using LandmarkHunt.Data;

namespace LandmarkHunt.Models
{
    public class LocDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;

        public Location toLocation()
        {
            Location loc = new Location();
            loc.Id = Id;
            loc.Name = Name;
            loc.Year = Year;
            Latitude= Latitude.Replace(".",",");
            Longitude = Longitude.Replace(".", ",");
            loc.Latitude = double.Parse(Latitude);
            loc.Longitude = double.Parse(Longitude);
            loc.PhotoUrl= PhotoUrl;
            return loc;

        }
    }

}

