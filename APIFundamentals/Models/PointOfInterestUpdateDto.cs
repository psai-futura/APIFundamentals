using System.ComponentModel.DataAnnotations;

namespace APIFundamentals.Models;

public class PointOfInterestUpdateDto
{
    [Required(ErrorMessage = "Please provide Name value")]
    [MaxLength(50)]
    public string Name { get; set; } = String.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
}