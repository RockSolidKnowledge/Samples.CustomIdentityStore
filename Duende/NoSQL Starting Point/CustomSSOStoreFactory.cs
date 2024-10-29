using Microsoft.Extensions.Options;
using NoSQLStartingPoint.Models;
using NoSQLStartingPoint.Stores;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint;

public class CustomSSOStoreFactory : ISSOStoreFactory
{
    private readonly IOptions<IdentityStoreDatabaseSettings> dbSettings;

    public CustomSSOStoreFactory(IOptions<IdentityStoreDatabaseSettings> dbSettings)
    {
        this.dbSettings = dbSettings ?? throw new ArgumentNullException(nameof(dbSettings));
    }
    
    public ISSOUserStore CreateUserStore()
    {
        return new UserStore(dbSettings);
    }

    public ISSORoleStore CreateRoleStore()
    {
        return new RoleStore(dbSettings);
    }

    public ISSOClaimTypeStore CreateClaimTypeStore()
    {
        return new ClaimTypeStore(dbSettings);
    }
}