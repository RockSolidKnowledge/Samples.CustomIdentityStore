using CustomDatabaseConnection;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.MigrationRunner;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAdminUI(
        options =>
        {
            options.DatabaseConnectionFactoryType = DatabaseConnectionFactoryType.Custom;

            // Uncomment this to stop migrations running automatically
            // The default is to run all migrations.
            //options.MigrationOptions = MigrationOptions.None;
        }
    )
    .WithConnectionFactory<CustomConnectionFactory>();

var app = builder.Build();

// Uncomment this to run migrations manually
//app.RunMigrations(MigrationType.All);

app.UseAdminUI();

app.Run();