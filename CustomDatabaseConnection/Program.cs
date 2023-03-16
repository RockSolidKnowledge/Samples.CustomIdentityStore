using CustomDatabaseConnection;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAdminUI(options =>
    {
        options.DatabaseConnectionFactoryType = DatabaseConnectionFactoryType.Custom;
    })
    .WithConnectionFactory<CustomConnectionFactory>();

var app = builder.Build();

await app.RunMigrationsAsync();

app.UseAdminUI();

app.Run();