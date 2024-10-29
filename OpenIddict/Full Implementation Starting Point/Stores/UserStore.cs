using Rsk.CustomIdentity.Interfaces;
using Rsk.CustomIdentity.Models;

namespace Full_Implementation.Stores;

public class UserStore : ISSOUserStore
{
    public Task<ISSOUser> CreateUser(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> CreateUserWithPassword(ISSOUser user, string password)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetUsersCount()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesUserExist(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> UpdateUser(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task HardDeleteUser(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> FindUserById(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> FindUserByUserName(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<FindUsersResult> FindUsers(UserSearch filter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTOTPEnabled(ISSOUser user)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> FindUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<ISSOUser> RemoveFederatedLogin(ISSOUser user, string loginProvider, string providerKey)
    {
        throw new NotImplementedException();
    }
    public Task<IList<ISSOUserLoginInfo>> GetFederatedLogins(ISSOUser user)
    {
        throw new NotImplementedException();
    }
}