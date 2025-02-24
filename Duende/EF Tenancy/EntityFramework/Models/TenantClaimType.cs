using System.ComponentModel.DataAnnotations;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.EntityFramework.Models;

public class TenantClaimType
{
    [Key]
    public string Id { get; set; }
    
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }
    public bool IsReserved { get; set; }
    public SSOClaimValueType ValueType { get; set; }
    public string RegularExpressionValidationRule { get; set; }
    public string RegularExpressionValidationFailureDescription { get; set; }
    public bool IsUserEditable { get; set; }
    
    public ICollection<TenantEnumClaimTypeValue> AllowedValues { get; set; }
}