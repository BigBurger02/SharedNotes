using System.ComponentModel.DataAnnotations;

namespace SharedNotes.Model;

public class Note
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200, ErrorMessage = "Title too long.")]
    public string Title { get; set; } = String.Empty;
    
    [StringLength(65530, ErrorMessage = "Title too long.")]
    public string? Body { get; set; } = String.Empty;
    
    public DateTime Created { get; set; }
    
    public DateTime LastEdit { get; set; }
}