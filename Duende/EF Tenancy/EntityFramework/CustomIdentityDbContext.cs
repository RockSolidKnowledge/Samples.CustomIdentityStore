using EF_Tenancy.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace EF_Tenancy.EntityFramework;
public class CustomIdentityDb : DbContext
{
    public CustomIdentityDb(DbContextOptions<CustomIdentityDb> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TenantEnumClaimTypeValue>(enumClaimTypeValue =>
        {
            enumClaimTypeValue.HasKey(ctv => new { ctv.ClaimTypeId, ctv.Value });
        });
    }

    public DbSet<TenantUser> Users { get; set; }

    public DbSet<TenantRole> Roles { get; set; }
    
    public DbSet<TenantClaim> UserClaims { get; set; }
    
    public DbSet<TenantClaimType> ClaimTypes { get; set; }
    public DbSet<TenantEnumClaimTypeValue> EnumClaimTypeAllowedValues { get; set; }
    
    public DbSet<Tenant> Tenants { get; set; }
}