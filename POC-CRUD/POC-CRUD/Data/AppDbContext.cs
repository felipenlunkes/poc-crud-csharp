using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using POC_CRUD.Models;

namespace POC_CRUD.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<HealthStatus> HealthStatuses { get; set; }
    
    // Informar ao EF Core que esses atributos são uma composição e devem ser mapeados na mesma tabela
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.OwnsOne(a => a.Phone, phone =>
            {
                phone.Property(p => p.CountryCode).HasColumnName("PhoneCountryCode");
                phone.Property(p => p.StateCode).HasColumnName("PhoneStateCode");
                phone.Property(p => p.Number).HasColumnName("PhoneNumber");
            });

            entity.OwnsOne(a => a.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("AddressStreet");
                address.Property(a => a.Number).HasColumnName("AddressNumber");
                address.Property(a => a.City).HasColumnName("AddressCity");
                address.Property(a => a.District).HasColumnName("AddressDistrict");
                address.Property(a => a.State).HasColumnName("AddressState");
                address.Property(a => a.Complement).HasColumnName("AddressComplement");
                address.Property(a => a.PostalCode).HasColumnName("AddressPostalCode");
            });

            entity.Property(a => a.Role).HasConversion(new EnumToStringConverter<AccountRoleEnum>());
        });
    }
}