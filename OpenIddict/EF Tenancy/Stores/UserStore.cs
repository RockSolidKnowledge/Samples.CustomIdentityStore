using EF_Tenancy.EntityFramework;
using EF_Tenancy.EntityFramework.Models;
using EF_Tenancy.Extensions;
using IdentityExpress.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rsk.CustomIdentity.Exceptions;
using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace EF_Tenancy.Stores;


public class UserStore : ISSOUserStore
{
    private readonly CustomIdentityDb _identityContext;
    private readonly IPasswordHasher<TenantUser> _passwordHasher;

    public UserStore(CustomIdentityDb identityContext)
    {
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
        _passwordHasher = new PasswordHasher<TenantUser>();
    }
    public async Task<ISSOUser> CreateUser(ISSOUser user)
    {
        var dbUser = user.ToDbUser();
        dbUser.ConcurrencyStamp = Guid.NewGuid().ToString();
        var tenantName =  user.Claims.FirstOrDefault(claim => claim.ClaimType == TenantClaimTypeConsts.TenantCTName)?.ClaimValue;
        dbUser.Tenant = await _identityContext.Tenants.FirstOrDefaultAsync(tenant => tenant.Name == tenantName);

        _identityContext.Users.Add(dbUser);

        await _identityContext.SaveChangesAsync();

        var userInDb = await _identityContext.Users.FirstOrDefaultAsync(dbUsers => dbUsers.UserName == user.UserName);
        return userInDb.ToStoreUser();
    }

    public async Task<ISSOUser> CreateUserWithPassword(ISSOUser user, string password)
    {
        var dbUser = user.ToDbUser();
        dbUser.ConcurrencyStamp = Guid.NewGuid().ToString();
        dbUser.Password = _passwordHasher.HashPassword(dbUser, password);
        var tenantName =  user.Claims.FirstOrDefault(claim => claim.ClaimType == TenantClaimTypeConsts.TenantCTName)?.ClaimValue;
        
        dbUser.Tenant = await _identityContext.Tenants.FirstOrDefaultAsync(tenant => tenant.Name == tenantName);

        _identityContext.Users.Add(dbUser);

        await _identityContext.SaveChangesAsync();

        var userInDb = await _identityContext.Users.FirstOrDefaultAsync(dbUsers => dbUsers.UserName == user.UserName);
        return userInDb.ToStoreUser();
    }

    public async Task<int> GetUsersCount()
    {
        return await _identityContext.Users.CountAsync();
    }

    public Task<bool> DoesUserExist(ISSOUser user)
    {
        return Task.FromResult(_identityContext.Users.Any(dbUser => dbUser.UserName == user.UserName));
    }

    public async Task<ISSOUser> UpdateUser(ISSOUser user)
    {
        var dbUser = await _identityContext.Users.Include(dbUser => dbUser.Claims).Include(dbUser => dbUser.Roles).Include(dbUser => dbUser.Tenant).FirstOrDefaultAsync(dbUser => dbUser.UserId == user.Id);
        dbUser.Email = user.Email;
        dbUser.FirstName = user.FirstName;
        dbUser.UserName = user.UserName;
        dbUser.TwoFactorEnabled = user.TwoFactorEnabled;
        dbUser.LastName = user.LastName;
        dbUser.ConcurrencyStamp = Guid.NewGuid().ToString();

        var tenantClaim = user.Claims.FirstOrDefault(userClaim => userClaim.ClaimType == TenantClaimTypeConsts.TenantCTName)?.ClaimValue;
        var dbTenant = _identityContext.Tenants.FirstOrDefault(tenant => tenant.Name == tenantClaim);

        dbUser.Tenant = dbTenant;

        await _identityContext.SaveChangesAsync();

        return dbUser.ToStoreUser();
    }

    public async Task DeleteUser(ISSOUser user)
    {
        var foundUser = await _identityContext.Users.FirstAsync(dbUser => dbUser.UserName == user.UserName);
        if (foundUser == null)
        {
            throw new UserNotFoundException(user.UserName);
        }

        foundUser.IsDeleted = true;

        _identityContext.Users.Update(foundUser);

        await _identityContext.SaveChangesAsync();
    }

    public async Task HardDeleteUser(ISSOUser user)
    {
        var foundUser = await _identityContext.Users.FindAsync(user.Id);
        if (foundUser == null)
        {
            throw new UserNotFoundException(user.UserName);
        }

        _identityContext.Users.Remove(foundUser);
        
        await _identityContext.SaveChangesAsync();
    }

    public async Task<ISSOUser> FindUserById(string userId)
    {
        var dbUser = await _identityContext.Users.Include(user => user.Claims).Include(user => user.Roles).Include(user => user.Tenant).FirstOrDefaultAsync(user => user.UserId == userId);
        if (dbUser == null)
        {
            throw new UserNotFoundException(userId);
        }

        var storeUser = dbUser.ToStoreUser();
        storeUser.Claims.Add(new SSOClaim(TenantClaimTypeConsts.TenantCTName, dbUser.Tenant.Name));

        return storeUser;
    }

    public async Task<ISSOUser> FindUserByUserName(string userName)
    {
        var dbUser = await _identityContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        if (dbUser == null)
        {
            throw new UserNotFoundException(userName);
        }

        var storeUser = dbUser.ToStoreUser();
        storeUser.Claims.Add(new SSOClaim(TenantClaimTypeConsts.TenantCTName, dbUser.Tenant.Name));

        return storeUser;
    }

    public Task<FindUsersResult> FindUsers(UserSearch filter)
    {
        var dbUsers = _identityContext.Users.Include(user => user.Claims).Include(user => user.Roles).Include(user => user.Tenant).ToList();
        if (dbUsers == null)
        {
            throw new UserNotFoundException();
        }

        if (!string.IsNullOrEmpty(filter.ToMatch))
        {
            dbUsers = dbUsers.Where(user => user.Email == filter.ToMatch).ToList();
        }

        var storeUsers = dbUsers.Select(x => x.ToStoreUser());
        foreach (var customSSOUser in storeUsers)
        {
            var userTenant = dbUsers.FirstOrDefault(dbUser => dbUser.UserId == customSSOUser.Id).Tenant.Name;
            customSSOUser.Claims.Add(new SSOClaim(TenantClaimTypeConsts.TenantCTName, userTenant));
        }

        return Task.FromResult(new FindUsersResult(storeUsers.ToList(), storeUsers.Count()));
    }

    public async Task<bool> IsTOTPEnabled(ISSOUser user)
    {
        var dbUser = await _identityContext.Users.FindAsync(user.Id);
        if (dbUser == null)
        {
            throw new UserNotFoundException(user.Id);
        }

        return dbUser.TwoFactorEnabled;
    }

    public async Task<ISSOUser> FindUserByEmail(string email)
    {
        var dbUser = await _identityContext.Users.Include(user => user.Claims).Include(user => user.Roles).Include(user => user.Tenant).FirstOrDefaultAsync(user => user.Email == email);
        if (dbUser == null)
        {
            throw new UserNotFoundException(email);
        }

        var storeUser = dbUser.ToStoreUser();
        storeUser.Claims.Add(new SSOClaim("Tenant", dbUser.Tenant?.Name));

        return storeUser;
    }
    
    public async Task<ISSOUser> RemoveFederatedLogin(ISSOUser user, string loginProvider, string providerKey)
    {
        var dbUser = await _identityContext.Users.FindAsync(user.Id);
        if (dbUser == null)
        {
            throw new UserNotFoundException(user.UserName);
        }

        return dbUser.ToStoreUser();
    }

    public Task<IList<ISSOUserLoginInfo>> GetFederatedLogins(ISSOUser user)
    {
        IList<ISSOUserLoginInfo> blankList = new List<ISSOUserLoginInfo>();
        return Task.FromResult(blankList);
    }
}