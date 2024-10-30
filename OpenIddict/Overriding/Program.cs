using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;
using Overriding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services
    .AddAdminUI(options =>
    {
        options.IdentityType = IdentityType.DefaultIdentity;
    })
    .WithIdentityStore<CustomStoreFactory>();

var app = builder.Build();

app.UseAdminUI();

app.Run();