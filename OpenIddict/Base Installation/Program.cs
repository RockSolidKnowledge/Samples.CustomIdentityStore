using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAdminUI();

var app = builder.Build();

app.UseAdminUI();

app.Run();
