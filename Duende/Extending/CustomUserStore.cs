using IdentityExpress.Manager.BusinessLogic.Interfaces.Identity;
using IdentityExpress.Manager.BusinessLogic.Logic.Services;
using IdentityExpress.Manager.BusinessLogic.Logic.Services.UserQueries;
using Microsoft.AspNetCore.Identity;
using Rsk.CustomIdentity.Interfaces;

namespace Extending;

public class CustomUserStore : SSOUserStore
{
    private readonly INotificationService _notificationService;

    public CustomUserStore(INotificationService notificationService, IIdentityUnitOfWorkFactory factory, ILookupNormalizer normaliser, IUserQueryFactory userQueryFactory) : base(factory, normaliser, userQueryFactory)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public override Task<ISSOUser> CreateUserWithPassword(ISSOUser user, string password)
    {
        var createdUser = base.CreateUserWithPassword(user, password);
        
        _notificationService.Dispatch("CreateUser");
        
        return createdUser;
    }
}