namespace APIFundamentals.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }

    public List<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();

    public int NumberOfPointsOfInterest
    {
        get
        {
            return PointsOfInterest.Count;
        }
    }
    
}