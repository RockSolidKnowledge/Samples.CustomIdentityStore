using EF_Tenancy.EntityFramework;
using EF_Tenancy.Extensions;
using Microsoft.EntityFrameworkCore;
using Rsk.CustomIdentity.Exceptions;
using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace EF_Tenancy.Stores;

public class RoleStore : ISSORoleStore
{
    private readonly CustomIdentityDb _identityContext;

    public RoleStore(CustomIdentityDb identityContext)
    {
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
    }
    public async Task<ISSORole> CreateRole(ISSORole role)
    {
        var foundRole = await _identityContext.Roles.FindAsync(role.Id);
        if (foundRole == null)
        {
            _identityContext.Roles.Add(role.ToDbRole());
        }

        return role;
    }

    public async Task DeleteRole(ISSORole role)
    {
        var foundRole = await _identityContext.Roles.FindAsync(role.Id);
        if (foundRole == null)
        {
            throw new RoleNotFoundException();
        }

        _identityContext.Roles.Remove(foundRole);
    }

    public async Task<ISSORole> GetRoleById(string id)
    {
        var foundRole = await _identityContext.Roles.FindAsync(id);
        if (foundRole == null)
        {
            throw new RoleNotFoundException();
        }

        return foundRole;
    }

    public async Task<ISSORole> UpdateRole(ISSORole role)
    {
        var dbRole = await _identityContext.Roles.FindAsync(role.Id);
        dbRole.Description = role.Description;
        dbRole.NonEditable = role.NonEditable;
        dbRole.Name = role.Name;

        _identityContext.Roles.Update(dbRole);
        
        await _identityContext.SaveChangesAsync();
        
        return dbRole;
    }

    public async Task<IEnumerable<ISSORole>> Get(RoleSearchFilter filter)
    {
        var roles = _identityContext.Roles;
        if (string.IsNullOrEmpty(filter.Name))
        {
            return await roles.ToListAsync();
        }
        return await _identityContext.Roles.Where(role => role.Name == filter.Name).ToListAsync();
    }

    public async Task AddUsersToRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        var dbRole = await _identityContext.Roles.FindAsync(role.Id);

        if (dbRole == null)
        {
            throw new RoleNotFoundException();
        }

        var usersToApply = users.Where(storeUser => dbRole.Users.All(dbUser => dbUser.UserId != storeUser.Id));
        
        foreach (var ssoUser in usersToApply)
        {
            dbRole.Users.Add(ssoUser.ToDbUser());
        }

        await _identityContext.SaveChangesAsync();
    }

    public async Task RemoveUsersFromRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        var dbRole = await _identityContext.Roles.FindAsync(role.Id);

        if (dbRole == null)
        {
            throw new RoleNotFoundException();
        }

        var usersToApply = users.Where(storeUser => dbRole.Users.All(dbUser => dbUser.UserId != storeUser.Id));
        
        foreach (var ssoUser in usersToApply)
        {
            dbRole.Users.Remove(ssoUser.ToDbUser());
        }

        await _identityContext.SaveChangesAsync();
    }

    public async Task AddUserToRoles(ISSOUser user, IEnumerable<string> roles)
    {
        var rolesToAddUserTo = _identityContext.Roles.Where(role => roles.Contains(role.Id));
        var userToAdd = await _identityContext.Users.FindAsync(user.Id);

        await rolesToAddUserTo.ForEachAsync(tenantRole => tenantRole.Users.Add(userToAdd));
        
        await _identityContext.SaveChangesAsync();
    }

    public async Task RemoveUserFromRoles(ISSOUser user, IEnumerable<string> roles)
    {
        var rolesToRemoveUserFrom = _identityContext.Roles.Where(role => roles.Contains(role.Id));
        var userToRemove = await _identityContext.Users.FindAsync(user.Id);

        await rolesToRemoveUserFrom.ForEachAsync(role => role.Users.Remove(userToRemove));

        await _identityContext.SaveChangesAsync();
    }

    public async Task AddAndRemoveUsers(ISSORole role, IEnumerable<string> usersToAdd, IEnumerable<string> usersToRemove)
    {
        var roleToAugment = await _identityContext.Roles.FindAsync(role.Id);
        
        var dbUsersToAdd = _identityContext.Users.Where(user => usersToAdd.Contains(user.UserId));

        await dbUsersToAdd.ForEachAsync(dbUser => roleToAugment.Users.Add(dbUser));

        var dbUsersToRemove = _identityContext.Users.Where(user => usersToRemove.Contains(user.UserId));
        await dbUsersToRemove.ForEachAsync(dbUser => roleToAugment.Users.Remove(dbUser));

        await _identityContext.SaveChangesAsync();
    }

    public async Task<FindUsersResult> GetUsersByRole(string roleId, IPagination pagination, string query)
    {
        var role = await _identityContext.Roles.FindAsync(roleId);

        return new FindUsersResult(role.Users.Select(user => user.ToStoreUser()).ToList(),
            role.Users.Count);
    }

    public async Task<FindUsersWithRoleStatusResult> FindUsersWithRoleStatus(string roleId, string searchTerm, UserState userState, IPagination pagination)
    {
        var users = _identityContext.Users
            .Include(user => user.Roles)
            .Where(user => user.IsBlocked == !userState.Active)
            .ToList();

        return new FindUsersWithRoleStatusResult(users.Select(user => new UserWithRoleStatus(user.ToStoreUser(), user.Roles.Any(role => role.Id == roleId))), users.Count);
    }

    public async Task<IEnumerable<ISSORole>> FindRolesByUser(ISSOUser user)
    {
        var dbUser = await _identityContext.Users.FindAsync(user.Id);
        return dbUser.Roles;
    }
}