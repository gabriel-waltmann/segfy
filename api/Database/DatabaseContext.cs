using Microsoft.EntityFrameworkCore;

namespace api.CarInsurancePolicy;

public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options)
    : base(options) { }

    public DbSet<CarInsurancePolicyEntity> Policies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CarInsurancePolicyEntity>()
            .HasKey(p => p.Number);
        
        modelBuilder.Entity<CarInsurancePolicyEntity>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
    }
}