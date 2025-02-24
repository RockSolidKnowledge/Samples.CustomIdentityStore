using EF_Tenancy.EntityFramework;
using EF_Tenancy.Stores;
using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy;

public class CustomSSOStoreFactory : ISSOStoreFactory
{
    private readonly CustomIdentityDb _identityDb;

    public CustomSSOStoreFactory(CustomIdentityDb identityDb)
    {
        _identityDb = identityDb ?? throw new ArgumentNullException(nameof(identityDb));
    }
    
    public ISSOUserStore CreateUserStore()
    {
        return new UserStore(_identityDb);
    } 

    public ISSORoleStore CreateRoleStore()
    {
        return new RoleStore(_identityDb);
    }

    public ISSOClaimTypeStore CreateClaimTypeStore()
    {
        return new ClaimTypeStore(_identityDb);
    }
}