using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace Full_Implementation.Stores;

public class RoleStore : ISSORoleStore
{
    public Task<ISSORole> CreateRole(ISSORole role)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRole(ISSORole role)
    {
        throw new NotImplementedException();
    }

    public Task<ISSORole> GetRoleById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ISSORole> UpdateRole(ISSORole role)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSORole>> Get(RoleSearchFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task AddUsersToRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUsersFromRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        throw new NotImplementedException();
    }
    
    public Task AddUsersToRole(ISSORole role, IList<ISSOUser> users)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUsersFromRole(ISSORole role, IList<ISSOUser> users)
    {
        throw new NotImplementedException();
    }

    public Task AddUserToRoles(ISSOUser user, IEnumerable<string> roles)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromRoles(ISSOUser user, IEnumerable<string> roles)
    {
        throw new NotImplementedException();
    }

    public Task AddAndRemoveUsers(ISSORole role, IEnumerable<string> usersToAdd, IEnumerable<string> usersToRemove)
    {
        throw new NotImplementedException();
    }

    public Task<FindUsersResult> GetUsersByRole(string roleId, IPagination pagination, string query)
    {
        throw new NotImplementedException();
    }

    public Task<FindUsersInRoleResult> FindUsersInRole(string roleId, string searchTerm, UserState userState, IPagination pagination)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ISSORole>> FindRolesByUser(ISSOUser user)
    {
        throw new NotImplementedException();
    }
}