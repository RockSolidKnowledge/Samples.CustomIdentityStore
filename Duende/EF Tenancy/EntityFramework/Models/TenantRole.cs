using System.ComponentModel.DataAnnotations;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.EntityFramework.Models;

public class TenantRole : ISSORole
{
    [Key]
    public string Id { get; set; }
    public string Description { get; set; }
    public bool NonEditable { get; set; }
    public string Name { get; set; }
    public virtual ICollection<TenantUser> Users { get; set; }
    public ICollection<ISSOClaim> Claims { get; set; }
}