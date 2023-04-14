namespace LandmarkHunt.Services;

public class ScoreCalculator
{
    public static int GetScore(int locYear, double locLatitude, double locLongitude, int guessYear, double guessLatitude, double guessLongitude, string hardness)
        => DistanceScore(locLatitude, locLongitude, guessLatitude, guessLongitude, hardness) + YearScore(guessYear, locYear, hardness);

    public static double DistanceTo(double locLatitude, double locLongitude, double guessLatitude, double guessLongitude, char unit = 'K')
    {
        double rlat1 = Math.PI * locLatitude / 180;
        double rlat2 = Math.PI * guessLatitude / 180;
        double theta = locLongitude - guessLongitude;
        double rtheta = Math.PI * theta / 180;
        double dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        return unit switch
        {
            //Kilometers -> default
            'K' => dist * 1.609344,
            //Nautical Miles 
            'N' => dist * 0.8684,
            //Miles
            'M' => dist,
            _ => dist,
        };
    }

    private static int YearScore(int guess, int actual, string hardness)
    {
        var multiplier = hardness switch
        {
            //medium
            "Medium" => 2,
            //hard
            "Hard" => 3,
            //easy
            _ => (double)1,
        };
        double modifier = ((double)(2500 - actual)) / (10 * multiplier);
        double score = Math.Exp(-0.5 * (Math.Pow((guess - actual) / modifier, 2)));
        return (int)(score * multiplier * 500);
    }

    private static int DistanceScore(double locLatitude, double locLongitude, double guessLatitude, double guessLongitude, string hardness)
    {
        var multiplier = hardness switch
        {
            //medium
            "Medium" => 2,
            //hard
            "Hard" => 3,
            //easy
            _ => (double)1,
        };
        double distance = DistanceTo(locLatitude, locLongitude, guessLatitude, guessLongitude);
        double score = Math.Max(Math.Min((200.1 / multiplier - distance) / (200 / multiplier), 500), 0);
        return (int)(score * multiplier * 500);
    }
}
