using Rsk.CustomIdentity.Interfaces;

namespace Full_Implementation.Stores;

public class ClaimTypeStore : ISSOClaimTypeStore
{
    public Task<ISSOClaimType> GetClaimTypeByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> GetClaimTypesThatContainName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> GetUserEditableClaimTypes()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> GetRequiredClaimTypes()
    {
        throw new NotImplementedException();
    }

    public Task<ISSOClaimType> CreateClaimType(ISSOClaimType claimType)
    {
        throw new NotImplementedException();
    }

    public Task DeleteClaimType(ISSOClaimType claimType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSOClaimType>> GetAllClaimTypes()
    {
        throw new NotImplementedException();
    }

    public Task<ISSOClaimType> GetClaimTypeById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOClaimType> UpdateClaimType(ISSOClaimType claimType)
    {
        throw new NotImplementedException();
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