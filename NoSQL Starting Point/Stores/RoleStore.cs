using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSQLStartingPoint.Models;
using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace NoSQLStartingPoint.Stores;

public class RoleStore : ISSORoleStore
{
    private readonly IMongoCollection<CustomSSOUser> dbUsers;
    private readonly IMongoCollection<CustomSSORole> dbRoles;
    
    public RoleStore(IOptions<IdentityStoreDatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);

        dbUsers = mongoDatabase.GetCollection<CustomSSOUser>(
            dbSettings.Value.UsersCollectionName);
        
        dbRoles = mongoDatabase.GetCollection<CustomSSORole>(
            dbSettings.Value.RolesCollectionName);
    }
    
    public async Task<ISSORole> CreateRole(ISSORole role)
    {
        await dbRoles.InsertOneAsync(new CustomSSORole
        {
            Name = role.Name,
            Description = role.Description,
            NonEditable = role.NonEditable
        });

        var rolesInsertedName = await dbRoles.FindAsync(r => r.Name == role.Name);

        return await rolesInsertedName.FirstOrDefaultAsync();
    }

    public async Task DeleteRole(ISSORole role)
    {
        await dbRoles.FindOneAndDeleteAsync(r => r.Id == role.Id);
    }

    public async Task<ISSORole> GetRoleById(string id)
    {
        var findResult = await dbRoles.FindAsync(r => r.Id == id);

        return await findResult.FirstOrDefaultAsync();
    }

    public async Task<ISSORole> UpdateRole(ISSORole role)
    {
        var updatedRole = MapRole(role);

        await dbRoles.FindOneAndReplaceAsync(r => r.Id == role.Id, updatedRole);

        var findResult = await dbRoles.FindAsync(r => r.Id == role.Id);

        return await findResult.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ISSORole>> Get(RoleSearchFilter filter)
    {
        var allRoles = await dbRoles.FindAsync(_ => true);
        
        IEnumerable<ISSORole> filteredRoles = await allRoles.ToListAsync();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            filteredRoles = filteredRoles.Where(r => r.Name == filter.Name);
        }

        if (filter.Ordering.Any())
        {
            var firstOrdering = filter.Ordering.First();

            filteredRoles = firstOrdering switch
            {
                RoleOrderBy.NameAscending => filteredRoles.OrderBy(r => r.Name),
                RoleOrderBy.NameDescending => filteredRoles.OrderByDescending(r => r.Name),
                _ => filteredRoles
            };
        }

        if (filter.Fields.Any())
        {
            var includeId = filter.Fields.Contains(RoleFields.Id);
            var includeName = filter.Fields.Contains(RoleFields.Name);
            var includeDescription = filter.Fields.Contains(RoleFields.Description);

            filteredRoles = filteredRoles.Select(fr => new CustomSSORole
            {
                Id = (includeId ? fr.Id : null)!,
                Name = (includeName ? fr.Name : null)!,
                Description = (includeDescription ? fr.Description : null)!
            });
        }
        
        return filteredRoles;
    }

    public async Task<IEnumerable<ISSORole>> GetAllRoles()
    {
        var allRoles = await dbRoles.FindAsync(_ => true);
        return await allRoles.ToListAsync();
    }

    public async Task AddUsersToRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        var roleQuery = await dbRoles.FindAsync(r => r.Id == role.Id);
        var roleToAdd = await roleQuery.FirstOrDefaultAsync();

        foreach (var user in users)
        {
            var userQuery = await dbUsers.FindAsync(u => u.Id == user.Id);
            var userToAddRoleTo = await userQuery.FirstOrDefaultAsync();
            
            userToAddRoleTo.Roles.Add(roleToAdd);

            await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRoleTo.Id, userToAddRoleTo);
        }
    }

    public async Task RemoveUsersFromRole(ISSORole role, IEnumerable<ISSOUser> users)
    {
        var roleQuery = await dbRoles.FindAsync(r => r.Id == role.Id);
        var roleToAdd = await roleQuery.FirstOrDefaultAsync();

        foreach (var user in users)
        {
            var userQuery = await dbUsers.FindAsync(u => u.Id == user.Id);
            var userToAddRoleTo = await userQuery.FirstOrDefaultAsync();
            
            userToAddRoleTo.Roles.Remove(roleToAdd);

            await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRoleTo.Id, userToAddRoleTo);
        }
    }

    public async Task AddUserToRoles(ISSOUser user, IEnumerable<string> roles)
    {
        var userQuery = await dbUsers.FindAsync(u => u.Id == user.Id);
        var userToAddRolesTo = await userQuery.FirstOrDefaultAsync();

        foreach (var role in roles)
        {
            var roleQuery = await dbRoles.FindAsync(r => r.Name == role);
            var roleToAddToUser = await roleQuery.FirstOrDefaultAsync();
            
            userToAddRolesTo.Roles.Add(roleToAddToUser);
        }
        
        await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRolesTo.Id, userToAddRolesTo);
    }

    public async Task RemoveUserFromRoles(ISSOUser user, IEnumerable<string> roles)
    {
        var userQuery = await dbUsers.FindAsync(u => u.Id == user.Id);
        var userToAddRolesTo = await userQuery.FirstOrDefaultAsync();

        foreach (var role in roles)
        {
            var roleQuery = await dbRoles.FindAsync(r => r.Name == role);
            var roleToAddToUser = await roleQuery.FirstOrDefaultAsync();
            
            userToAddRolesTo.Roles.Add(roleToAddToUser);
        }
        
        await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRolesTo.Id, userToAddRolesTo);
    }

    public async Task AddAndRemoveUsers(ISSORole role, IEnumerable<string> usersToAdd, IEnumerable<string> usersToRemove)
    {
        var roleQuery = await dbRoles.FindAsync(r => r.Id == role.Id);
        var roleToAddOrRemove = await roleQuery.FirstOrDefaultAsync();
        
        foreach (var userId in usersToAdd)
        {
            var userQuery = await dbUsers.FindAsync(u => u.Id == userId);
            var userToAddRoleTo = await userQuery.FirstOrDefaultAsync();
            
            userToAddRoleTo.Roles.Add(roleToAddOrRemove);

            await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRoleTo.Id, userToAddRoleTo);
        }
        
        foreach (var userId in usersToRemove)
        {
            var userQuery = await dbUsers.FindAsync(u => u.Id == userId);
            var userToAddRoleTo = await userQuery.FirstOrDefaultAsync();
            
            userToAddRoleTo.Roles.Remove(roleToAddOrRemove);

            await dbUsers.FindOneAndReplaceAsync(u => u.Id == userToAddRoleTo.Id, userToAddRoleTo);
        }
    }

    public async Task<FindUsersResult> GetUsersByRole(string roleId, IPagination pagination, string query)
    {
        var allUsers = await dbUsers.FindAsync(_ => true);
        var allUsersList = await allUsers.ToListAsync();

        var filteredUsers = allUsersList.Where(u => u.Roles.Any(r => r.Id == roleId));

        var totalUsers = filteredUsers.Count();
        
        var pagesToSkip = pagination.Page == 0 ? 0 : pagination.Page - 1;
        filteredUsers = filteredUsers.Skip(pagesToSkip * pagination.PageSize).Take(pagination.PageSize);

        return new FindUsersResult(filteredUsers, totalUsers);
    }

    public async Task<FindUsersWithRoleStatusResult> FindUsersWithRoleStatus(string roleId, string searchTerm, UserState userState, IPagination pagination)
    {
        var allUsers = await (await dbUsers.FindAsync(_ => true)).ToListAsync();

        IEnumerable<CustomSSOUser> filteredUsers = allUsers;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var usersByQuery = await dbUsers.FindAsync(u => u.UserName == searchTerm || u.FirstName == searchTerm || u.LastName == searchTerm);
            filteredUsers = usersByQuery.ToList();
        }

        filteredUsers = TranslateUserState(filteredUsers, userState).ToList();

        var totalUsers = filteredUsers.Count();
        
        var pagesToSkip = pagination.Page == 0 ? 0 : pagination.Page - 1;
        filteredUsers = filteredUsers.Skip(pagesToSkip * pagination.PageSize).Take(pagination.PageSize);

        return new FindUsersWithRoleStatusResult(filteredUsers.Select(user => new UserWithRoleStatus(user, user.Roles.Any(ur => ur.Id == roleId))), totalUsers);
    }
    
    public async Task<IEnumerable<ISSORole>> FindRolesByUser(ISSOUser user)
    {
        var dbUser = await (await dbUsers.FindAsync(u => user.Id == u.Id)).FirstOrDefaultAsync();
        var userRoleIds = dbUser.Roles.Select(r => r.Id);

        var matchingRoles = await dbRoles.FindAsync(role => userRoleIds.Contains(role.Id));

        return await matchingRoles.ToListAsync();
    }

    private IEnumerable<CustomSSOUser> TranslateUserState(IEnumerable<CustomSSOUser> source, UserState state)
    {
        if (state.Active && state.Blocked && state.Deleted)
            return source;
        if (!state.Active && !state.Blocked && !state.Deleted)
            return source;

        if (state.Active && state.Blocked)
        {
            source = source.Where(x => !x.IsDeleted);
        }
        else if (state.Active && state.Deleted)
        {
            source = source.Where(x => !x.IsBlocked);
        }
        else if (state.Blocked && state.Deleted)
        {
            source = source.Where(x => x.IsDeleted || x.IsBlocked);
        }
        else if (state.Active)
        {
            source = source.Where(x => !x.IsDeleted && !x.IsBlocked);
        }
        else if (state.Blocked)
        {
            source = source.Where(x => x.IsBlocked && !x.IsDeleted);
        }
        else if (state.Deleted)
        {
            source = source.Where(x => x.IsDeleted);
        }

        return source;
    }
    
    private CustomSSORole MapRole(ISSORole role)
    {
        var mappedRole = new CustomSSORole
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            NonEditable = role.NonEditable
        };

        return mappedRole;
    }
}