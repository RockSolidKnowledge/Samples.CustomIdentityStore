using Full_Implementation.Stores;
using Rsk.CustomIdentity.Interfaces;

namespace Full_Implementation;

public class CustomSSOStoreFactory : ISSOStoreFactory
{
    public ISSOUserStore CreateUserStore()
    {
        return new UserStore();
    }

    public ISSORoleStore CreateRoleStore()
    {
        return new RoleStore();
    }

    public ISSOClaimTypeStore CreateClaimTypeStore()
    {
        return new ClaimTypeStore();
    }
}