using CustomDatabaseConnection;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.MigrationRunner;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAdminUI(
    //Uncomment this to use custom connection factory
    //options =>
    //{
    //    options.DatabaseConnectionFactoryType = DatabaseConnectionFactoryType.Custom;
    //}
    )
  //Uncomment this to use custom connection factory
  //.WithConnectionFactory<CustomConnectionFactory>()
    ;

var app = builder.Build();

//Uncomment this to run migrations
//await app.RunMigrationsAsync(MigrationType.All);

app.UseAdminUI();

app.Run();