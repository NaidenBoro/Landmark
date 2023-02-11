namespace LandmarkHunt.Data;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;

    public virtual ICollection<UserGuess> UserGuesses { get; set; } = null!;
}
