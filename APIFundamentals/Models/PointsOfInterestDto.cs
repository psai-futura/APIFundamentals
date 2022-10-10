namespace APIFundamentals.Models;

public class PointsOfInterestDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public string? Description { get; set; }
    
}