using EF_Tenancy.EntityFramework;
using EF_Tenancy.EntityFramework.Models;
using EF_Tenancy.Extensions;
using Microsoft.EntityFrameworkCore;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.Stores;

public class ClaimTypeStore : ISSOClaimTypeStore
{
    private readonly CustomIdentityDb _identityContext;

    public ClaimTypeStore(CustomIdentityDb identityContext)
    {
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
    }

    public Task<ISSOClaimType> CreateClaimType(ISSOClaimType claimType)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteClaimType(ISSOClaimType claimType)
    {
        var ct = await _identityContext.ClaimTypes.FindAsync(claimType.Id);
        _identityContext.ClaimTypes.Remove(ct);
    }

    public async Task<IEnumerable<ISSOClaimType>> GetAllClaimTypes()
    {
        var ctList = _identityContext.ClaimTypes.Include(claimType => claimType.AllowedValues)
            .Select(dbCt => dbCt.ToStoreClaimType()).ToList();

        var tenantCt = ctList.FirstOrDefault(ctList => ctList.Id == TenantClaimTypeConsts.TenantCTId);
        tenantCt.AllowedValues = _identityContext.Tenants.Select(x => x.Name).ToList();
        
        return ctList;
    }
    
    public async Task<ISSOClaimType> UpdateClaimType(ISSOClaimType claimType)
    {
        var dbCt = await _identityContext.ClaimTypes.Include(claimType => claimType.AllowedValues)
            .FirstOrDefaultAsync(claimType => claimType.Id == claimType.Id);

        dbCt.Description = claimType.Description;
        dbCt.Name = claimType.Name;
        dbCt.DisplayName = claimType.DisplayName;
        dbCt.IsRequired = claimType.IsRequired;
        dbCt.IsReserved = claimType.IsReserved;
        dbCt.IsUserEditable = claimType.IsUserEditable;
        dbCt.ValueType = claimType.ValueType;
        dbCt.RegularExpressionValidationRule = claimType.RegularExpressionValidationRule;
        dbCt.RegularExpressionValidationFailureDescription = claimType.RegularExpressionValidationFailureDescription;

        dbCt.AllowedValues = dbCt.AllowedValues
            .Where(enumClaimTypeValue => claimType.AllowedValues.Contains(enumClaimTypeValue.Value)).ToList();        

        var newValues = claimType.AllowedValues.Where(value => dbCt.AllowedValues.All(dbEnum => dbEnum.Value != value))
            .Select(newAllowedValue => new TenantEnumClaimTypeValue
            {
                ClaimTypeId = dbCt.Id,
                Value = newAllowedValue
            }).ToList();
        
        newValues.ForEach(value => dbCt.AllowedValues.Add(value));
        _identityContext.ClaimTypes.Update(dbCt);
        await _identityContext.SaveChangesAsync();

        return dbCt.ToStoreClaimType();
    }

    public Task<IEnumerable<ISSOClaimType>> GetListOfClaimTypesByNames(IEnumerable<string> claimTypeNames)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> BulkUpdateClaimTypes(IEnumerable<ISSOClaimType> claimTypesToUpdate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> BulkCreateClaimTypes(IEnumerable<ISSOClaimType> claimTypesToAdd)
    {
        throw new NotImplementedException();
    }
}