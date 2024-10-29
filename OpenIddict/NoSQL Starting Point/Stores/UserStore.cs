using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSQLStartingPoint.Models;
using Rsk.CustomIdentity.Exceptions;
using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace NoSQLStartingPoint.Stores;

public class UserStore : ISSOUserStore
{
    private readonly IMongoCollection<CustomSSOUser> dbUsers;
    private readonly PasswordHasher<CustomSSOUser> passwordHasher;

    public UserStore(IOptions<IdentityStoreDatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);
    
        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);

        dbUsers = mongoDatabase.GetCollection<CustomSSOUser>(
            dbSettings.Value.UsersCollectionName);
        
        passwordHasher = new PasswordHasher<CustomSSOUser>();
    }
    
    public async Task<ISSOUser> CreateUser(ISSOUser user)
    {
        return await CreateUserInternal(user, null);
    }

    public async Task<ISSOUser> CreateUserWithPassword(ISSOUser user, string password)
    {
        return await CreateUserInternal(user, password);
    }

    public async Task<int> GetUsersCount()
    {
        return (int)await dbUsers.CountDocumentsAsync(filter: null);
    }

    public async Task<bool> DoesUserExist(ISSOUser user)
    {
        var dbUsers = await this.dbUsers.FindAsync(u => u.Id == user.Id);

        var anyUsers = await dbUsers.AnyAsync();

        return anyUsers;
    }

    public async Task<ISSOUser> UpdateUser(ISSOUser user)
    {
        var mappedUpdatedUser = await MapUser(user);
        
        if (!await DoesUserExist(mappedUpdatedUser))
        {
            throw new UserUpdateFailureException("User update failed");
        }

        await dbUsers.FindOneAndReplaceAsync(u => u.Id == mappedUpdatedUser.Id, mappedUpdatedUser);

        var updatedUserQuery = await dbUsers.FindAsync(u => u.UserName == mappedUpdatedUser.UserName);
        return await updatedUserQuery.FirstOrDefaultAsync();
    }

    public async Task DeleteUser(ISSOUser user)
    {
        var mappedUpdatedUser = await MapUser(user);
        mappedUpdatedUser.IsDeleted = true;
        
        await dbUsers.FindOneAndReplaceAsync(u => u.UserName == user.UserName, mappedUpdatedUser);
    }

    public async Task HardDeleteUser(ISSOUser user)
    {
        await dbUsers.FindOneAndDeleteAsync(u => u.UserName == user.UserName);
    }

    public async Task<IEnumerable<ISSOUser>> FindUsersByIds(IEnumerable<string> userId)
    {
        return await (await dbUsers.FindAsync(user => userId.Contains(user.Id))).ToListAsync();
    }

    public async Task<ISSOUser> FindUserById(string userId)
    {
        var filteredUsers = await dbUsers.FindAsync(u => u.Id == userId);

        return await filteredUsers.FirstOrDefaultAsync();
    }

    public async Task<ISSOUser> FindUserByUserName(string userName)
    {
        var filteredUsers = await dbUsers.FindAsync(u => u.UserName == userName);

        return await filteredUsers.FirstOrDefaultAsync();
    }

    public async Task<FindUsersResult> FindUsers(UserSearch filter)
    {
        var pagination = filter.Pagination;
        var state = filter.UserState;
        var query = filter.ToMatch;

        IEnumerable<ISSOUser> filteredUsers;

        // Apply filtering on string fields if available
        if (!string.IsNullOrWhiteSpace(query))
        {
            var usersByQuery = await dbUsers.FindAsync(u => u.UserName == query || u.FirstName == query || u.LastName == query);
            filteredUsers = await usersByQuery.ToListAsync();
        }
        else
        {
            var allUsers = await dbUsers.FindAsync(_ => true);
            filteredUsers = await allUsers.ToListAsync();
        }
        
        // Apply state filtering
        filteredUsers = TranslateUserState(filteredUsers, state).ToList();

        // Get total users
        var totalUsers = filteredUsers.Count();
        
        // Apply pagination
        var pagesToSkip = pagination.Page == 0 ? 0 : pagination.Page - 1;
        filteredUsers = filteredUsers.Skip(pagesToSkip * pagination.PageSize).Take(pagination.PageSize);

        return new FindUsersResult(filteredUsers, totalUsers);
    }

    public Task<bool> IsTOTPEnabled(ISSOUser user)
    {
        // This method is used to specify if a user has the ability to perform TOTP functionality. It has no consequence within AdminUI other than displaying a small message next to the user's name. 

        return Task.FromResult(false);
    }

    public async Task<ISSOUser> FindUserByEmail(string email)
    {
        var filteredUsers = await dbUsers.FindAsync(u => u.Email == email);

        return await filteredUsers.FirstOrDefaultAsync();
    }

    public async Task<ISSOUser> RemoveFederatedLogin(ISSOUser user, string loginProvider, string providerKey)
    {
        return user;
    }

    public async Task<IList<ISSOUserLoginInfo>> GetFederatedLogins(ISSOUser user)
    {
        return new List<ISSOUserLoginInfo>();
    }
    
    private async Task<ISSOUser> CreateUserInternal(ISSOUser user, string password)
    {
        var newUser = new CustomSSOUser
        {
            UserName = user.UserName,
            Email = user.Email,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = false,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsDeleted = false,
            LockoutEnabled = false,
        };

        if (!string.IsNullOrWhiteSpace(password))
        {
            newUser.Password = passwordHasher.HashPassword(newUser, password);
        }

        foreach (var claim in user.Claims)
        {
            newUser.Claims.Add(claim);
        }
        
        await dbUsers.InsertOneAsync(newUser);

        if (!await DoesUserExist(newUser))
        {
            throw new UserCreationFailureException("User creation failed");
        }
        
        var createdUserQuery = await dbUsers.FindAsync(u => u.UserName == newUser.UserName);
        return await createdUserQuery.FirstOrDefaultAsync();
    }
    
    private async Task<CustomSSOUser> MapUser(ISSOUser user)
    {
        var dbUser = await (await dbUsers.FindAsync(u => u.Id == user.Id)).FirstOrDefaultAsync();

        dbUser.Id = user.Id;
        dbUser.UserName = user.UserName;
        dbUser.Email = user.Email;
        dbUser.ConcurrencyStamp = Guid.NewGuid().ToString();
        dbUser.TwoFactorEnabled = user.TwoFactorEnabled;
        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.IsDeleted = user.IsDeleted;
        dbUser.LockoutEnabled = user.LockoutEnabled;
        
        dbUser.Claims.Clear();
        
        foreach (var claim in user.Claims)
        {
            dbUser.Claims.Add(new CustomSSOClaim
            {
                Id = claim.Id,
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue
            });
        }

        return dbUser;
    }
    
    private IEnumerable<ISSOUser> TranslateUserState(IEnumerable<ISSOUser> source, UserState state)
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
}