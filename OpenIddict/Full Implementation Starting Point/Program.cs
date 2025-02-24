using Full_Implementation;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAdminUI(options =>
    {
        options.IdentityType = IdentityType.CustomIdentity;
    })
    .WithIdentityStore<CustomSSOStoreFactory>();

var app = builder.Build();

app.UseAdminUI();

app.Run();