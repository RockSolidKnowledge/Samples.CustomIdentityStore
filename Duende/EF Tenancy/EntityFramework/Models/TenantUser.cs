using System.ComponentModel.DataAnnotations;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.EntityFramework.Models;

public class TenantUser
{
    [Key]
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    
    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; }
    public bool IsDeleted { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; }
    public string Password { get; set; }
    
    public virtual ICollection<TenantRole> Roles { get; set; }
    
    public virtual ICollection<TenantClaim> Claims { get; set; }
    
    public virtual Tenant? Tenant { get; set; }
}