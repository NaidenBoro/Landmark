using LandmarkHunt.Data;

namespace LandmarkHunt.Models;

public record GuessModel
(
    string ActualName,
    int ActualYear,
    double ActualLatitude,
    double ActualLongitude,
    int GuessYear,
    double GuessLatitude,
    double GuessLongitude,
    int Score,
    double Distance
 );