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
//app.RunMigrations(MigrationType.All);

app.UseAdminUI();

app.Run();