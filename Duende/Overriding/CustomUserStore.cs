using IdentityExpress.Manager.BusinessLogic.Interfaces.Identity;
using IdentityExpress.Manager.BusinessLogic.Logic.Services;
using IdentityExpress.Manager.BusinessLogic.Logic.Services.UserQueries;
using IdentityExpress.Manager.BusinessLogic.Mappers;
using Microsoft.AspNetCore.Identity;
using Rsk.CustomIdentity.Interfaces;

namespace Overriding;

public class CustomUserStore : SSOUserStore
{
    private readonly INotificationService _notificationService;
    private readonly IIdentityUnitOfWorkFactory _factory;

    public CustomUserStore(INotificationService notificationService, IIdentityUnitOfWorkFactory factory, ILookupNormalizer normaliser, IUserQueryFactory userQueryFactory) : base(factory, normaliser, userQueryFactory)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public override Task<ISSOUser> CreateUser(ISSOUser user)
    {
        var createdUser = base.CreateUser(user);
        
        _notificationService.Dispatch("CreateUser");
        
        return createdUser;
    }

    public override async Task DeleteUser(ISSOUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        using var uow = _factory.Create();
        
        var databaseUser = user.ToIdentityExpressUser();
        await uow.UserManager.HardDeleteAsync(databaseUser);

        await uow.Commit();
    }
}