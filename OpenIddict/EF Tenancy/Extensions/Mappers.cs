using EF_Tenancy.EntityFramework.Models;
using EF_Tenancy.Models;
using IdentityExpress.Identity;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.Extensions;

public static class Mappers
{
    public static ISSOUser ToStoreUser(this TenantUser dbUser)
    {
        var customUser = new CustomSSOUser
        {
            Id = dbUser.UserId,
            Email = dbUser.Email,
            FirstName = dbUser.FirstName,
            IsDeleted = dbUser.IsDeleted,
            LastName = dbUser.LastName,
            LockoutEnabled = dbUser.LockoutEnabled,
            UserName = dbUser.UserName,
            TwoFactorEnabled = dbUser.TwoFactorEnabled
        };

        customUser.Claims = dbUser.Claims.Select(dbClaim => dbClaim.ToStoreClaim()).ToList();

        return customUser;
    }

    public static ISSORole ToStoreRole(this TenantRole dbRole)
    {
        return new SSORole
        {
            Description = dbRole.Description,
            Id = dbRole.Id,
            Name = dbRole.Name,
            NonEditable = dbRole.NonEditable
        };
    }

    public static ISSOClaim ToStoreClaim(this TenantClaim dbClaim)
    {
        return new SSOClaim
        {
            Id = dbClaim.Id,
            ClaimType = dbClaim.ClaimType,
            ClaimValue = dbClaim.ClaimValue
        };
    }

    public static ISSOClaimType ToStoreClaimType(this TenantClaimType dbClaimType)
    {
        return new CustomSSOClaimType
        {
            Id = dbClaimType.Id,
            Description = dbClaimType.Description,
            Name = dbClaimType.Name,
            DisplayName = dbClaimType.DisplayName,
            IsRequired = dbClaimType.IsRequired,
            IsReserved = dbClaimType.IsReserved,
            IsUserEditable = dbClaimType.IsUserEditable,
            RegularExpressionValidationRule = dbClaimType.RegularExpressionValidationRule,
            RegularExpressionValidationFailureDescription = dbClaimType.RegularExpressionValidationFailureDescription,
            AllowedValues = dbClaimType.AllowedValues.Select(dbClaimTypeAllowedValue =>
                dbClaimTypeAllowedValue.Value).ToList(),
            ValueType = dbClaimType.ValueType
        };
    }

    public static TenantUser ToDbUser(this ISSOUser storeUser)
    {
        var dbUser = new TenantUser
        {
            UserId = storeUser.Id,
            Email = storeUser.Email,
            FirstName = storeUser.FirstName,
            IsDeleted = storeUser.IsDeleted,
            LastName = storeUser.LastName,
            LockoutEnabled = storeUser.LockoutEnabled,
            UserName = storeUser.UserName,
            TwoFactorEnabled = storeUser.TwoFactorEnabled
        };
        
        dbUser.Claims = storeUser.Claims.Select(dbClaim => dbClaim.ToDbClaim()).ToList();

        return dbUser;
    }
    
    public static TenantRole ToDbRole(this ISSORole storeRole)
    {
        return new TenantRole
        {
            Description = storeRole.Description,
            Id = storeRole.Id,
            Name = storeRole.Name,
            NonEditable = storeRole.NonEditable
        };
    }

    public static TenantClaim ToDbClaim(this ISSOClaim storeClaim)
    {
        return new TenantClaim
        {
            Id = storeClaim.Id,
            ClaimType = storeClaim.ClaimType,
            ClaimValue = storeClaim.ClaimValue
        };
    }
}