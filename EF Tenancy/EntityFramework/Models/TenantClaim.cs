using System.ComponentModel.DataAnnotations;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.EntityFramework.Models;

public class TenantClaim : ISSOClaim
{
    [Key]
    public int Id { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}