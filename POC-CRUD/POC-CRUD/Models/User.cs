using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace POC_CRUD.Models;

public class User
{
    
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [JsonProperty("email")]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    [JsonProperty("password")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "isAdmin is required")]
    [JsonProperty("isAdmin")]
    public bool IsAdmin { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}