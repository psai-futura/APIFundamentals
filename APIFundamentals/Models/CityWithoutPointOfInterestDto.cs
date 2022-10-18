namespace APIFundamentals.Models;

/// <summary>
/// A DTO for a city without Points of Interest
/// </summary>
public class CityWithoutPointOfInterestDto
{
    /// <summary>
    /// The Id of the city
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The Name of the city
    /// </summary>
    public string Name { get; set; } = String.Empty;
    
    /// <summary>
    /// The Description of the city
    /// </summary>
    public string? Description { get; set; }
}