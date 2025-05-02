using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace POC_CRUD.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "email is required")]
    [MaxLength(100)]
    [JsonProperty("email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password is required")]
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
    
    [JsonIgnore]
    public Boolean Removed { get; set; }
}