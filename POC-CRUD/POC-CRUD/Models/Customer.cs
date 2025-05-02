using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace POC_CRUD.Models;

public class Customer
{
    [Key]
    public Guid Id { get; set; }
    
    [JsonProperty("name")]
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters")]
    public string Name { get; set; }
    
    [JsonProperty("email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [JsonProperty("phone")]
    [Required(ErrorMessage = "Phone number is required")]
    [MaxLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
    public string Phone { get; set; }
    
    [JsonProperty("address")]
    [Required(ErrorMessage = "Address is required")]
    [MaxLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
    [MinLength(3, ErrorMessage = "Address cannot be less than 3 characters")]
    public string Address { get; set; }
    
    [JsonProperty("isActive")]
    public bool IsActive { get; set; }
    
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [JsonIgnore]
    public Boolean Removed { get; set; }
}