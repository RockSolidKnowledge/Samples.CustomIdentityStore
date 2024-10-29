using IdentityExpress.Manager.BusinessLogic;
using IdentityExpress.Manager.BusinessLogic.Interfaces.Identity;
using IdentityExpress.Manager.BusinessLogic.Logic.Services.UserQueries;
using Microsoft.AspNetCore.Identity;
using Rsk.CustomIdentity.Interfaces;

namespace Extending;

public class CustomStoreFactory : SSOStoreFactory
{
    private readonly INotificationService _notificationService;
    private readonly IIdentityUnitOfWorkFactory _factory;
    private readonly ILookupNormalizer _normaliser;
    private readonly IUserQueryFactory _userQueryFactory;

    public CustomStoreFactory(INotificationService notificationService, IIdentityUnitOfWorkFactory factory, ILookupNormalizer normaliser, IUserQueryFactory userQueryFactory)  : base(factory, userQueryFactory, normaliser)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _normaliser = normaliser ?? throw new ArgumentNullException(nameof(normaliser));
        _userQueryFactory = userQueryFactory ?? throw new ArgumentNullException(nameof(userQueryFactory));
    }
    
    public override ISSOUserStore CreateUserStore()
    {
        return new CustomUserStore(_notificationService, _factory, _normaliser, _userQueryFactory);
    }
}