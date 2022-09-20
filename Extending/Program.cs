using Extending;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddAdminUI().WithIdentityStore<CustomStoreFactory>();

var app = builder.Build();

app.UseAdminUI();

app.Run();