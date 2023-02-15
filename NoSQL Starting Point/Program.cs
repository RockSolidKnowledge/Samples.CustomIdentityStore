using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using NoSQLStartingPoint;
using NoSQLStartingPoint.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IdentityStoreDatabaseSettings>(builder.Configuration.GetSection("IdentityDatabase"));

builder.Services.AddAdminUI(options =>
{
    options.IdentityType = IdentityType.CustomIdentity;
    options.MigrationOptions = MigrationOptions.IdentityServerOnly;
}).WithIdentityStore<CustomSSOStoreFactory>();

var app = builder.Build();

// app.MapGet("/", async (IOptions<IdentityStoreDatabaseSettings> dbSettings) =>
// {
//     var mongoClient = new MongoClient(
//         dbSettings.Value.ConnectionString);
//     
//     var mongoDatabase = mongoClient.GetDatabase(
//         dbSettings.Value.DatabaseName);
//     
//     IMongoCollection<CustomSSOUser> dbUsers = mongoDatabase.GetCollection<CustomSSOUser>(
//         dbSettings.Value.UsersCollectionName);
//
//     IMongoCollection<CustomSSORole> dbRoles = mongoDatabase.GetCollection<CustomSSORole>(
//         dbSettings.Value.RolesCollectionName);
//
//     var adminRole = new CustomSSORole
//     {
//         Description = "Admin",
//         Name = "AdminUI Administrator",
//         NonEditable = true
//     };
//
//     var adminUser = new CustomSSOUser
//     {
//         Email = "sam@sam.com",
//         FirstName = "Sam",
//         IsDeleted = false,
//         LastName = "Jones",
//         LockoutEnabled = false,
//         TwoFactorEnabled = true,
//         UserName = "sam",
//         ConcurrencyStamp = Guid.NewGuid().ToString(),
//         Roles = new List<ISSORole>
//         {
//             adminRole
//         }
//     };
//     
//     adminUser.Password = new PasswordHasher<CustomSSOUser>().HashPassword(adminUser, "Password123!");
//
//     var administratorRoleQuery = await dbRoles.FindAsync(r => r.Name == "AdminUI Administrator");
//     
//     if (!await administratorRoleQuery.AnyAsync())
//     {
//         await dbRoles.InsertOneAsync(adminRole);
//     }
//
//     var userQuery = await dbUsers.FindAsync(u => u.UserName == "sam");
//     
//     if (!await userQuery.AnyAsync())
//     {
//         await dbUsers.InsertOneAsync(adminUser);
//     }
//
// });

app.UseAdminUI();

BsonClassMap.RegisterClassMap<CustomSSOUser>();
BsonClassMap.RegisterClassMap<CustomSSORole>();
BsonClassMap.RegisterClassMap<CustomSSOClaim>();
BsonClassMap.RegisterClassMap<CustomSSOClaimType>();

app.Run();