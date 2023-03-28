using CustomDatabaseConnection;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.MigrationRunner;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAdminUI(
    options =>
    {
        //Uncomment this to use custom connection factory
        //options.DatabaseConnectionFactoryType = DatabaseConnectionFactoryType.Custom;
        //Uncomment this to stop migrations running automatically
        //options.MigrationOptions = MigrationOptions.None;
    }
    )
  //Uncomment this to use custom connection factory
  //.WithConnectionFactory<CustomConnectionFactory>()
    ;

var app = builder.Build();

//Uncomment this to run migrations manually
//app.RunMigrations(MigrationType.All);

app.UseAdminUI();

app.Run();