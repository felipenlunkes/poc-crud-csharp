using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace POC_CRUD.Models;

public class Account
{
    
    [Key]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "userId is required")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "name is required")]
    [JsonProperty("name")]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [JsonProperty("businessName")]
    [MaxLength(100)]
    public string BusinessName { get; set; }
    
    [JsonProperty("cpf")]
    public string Cpf { get; set; }
    
    [JsonProperty("cnpj")]
    public string Cnpj { get; set; }
    
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [Required(ErrorMessage = "allowsAdvertising is required")]
    [JsonProperty("allowsAdvertising")]
    public bool AllowsAdvertising { get; set; }
    
    [JsonIgnore]
    public bool Removed { get; set; }
}