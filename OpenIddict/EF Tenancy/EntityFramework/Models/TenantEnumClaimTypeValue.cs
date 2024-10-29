using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.EntityFramework.Models;

public class TenantEnumClaimTypeValue
{
    public string ClaimTypeId { get; set; }
    public string Value { get; set; }
}