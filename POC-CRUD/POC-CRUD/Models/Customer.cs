using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace POC_CRUD.Models;

public class Customer
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
    [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters")]
    [JsonProperty("name")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    [MaxLength(100)]
    [JsonProperty("email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Phone number is required")]
    [MaxLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
    [JsonProperty("phone")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Address is required")]
    [MaxLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
    [MinLength(3, ErrorMessage = "Address cannot be less than 3 characters")]
    [JsonProperty("address")]
    public string Address { get; set; }
    [JsonProperty("isActive")]
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}