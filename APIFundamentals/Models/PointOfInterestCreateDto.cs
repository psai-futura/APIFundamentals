namespace APIFundamentals.Models;

public class PointOfInterestCreateDto
{
    public string Name { get; set; } = String.Empty;
    
    public string? Description { get; set; }
}