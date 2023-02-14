using LandmarkHunt.Data;
using System.Globalization;

namespace LandmarkHunt.Models
{
    public class LocModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;

        public void UpdateLocation(Location loc)
        {
            loc.Id = Id;
            loc.Name = Name;
            loc.Year = Year;/*
            Latitude= Latitude.Replace(".",",");
            Longitude = Longitude.Replace(".", ",");*/
            loc.Latitude = double.Parse(Latitude, CultureInfo.InvariantCulture);
            loc.Longitude = double.Parse(Longitude, CultureInfo.InvariantCulture);
            loc.PhotoUrl= PhotoUrl;
        }
    }

}

