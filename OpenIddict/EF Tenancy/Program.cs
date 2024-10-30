using EF_Tenancy;
using EF_Tenancy.EntityFramework;
using IdentityExpress.Manager.BusinessLogic.Configuration;
using IdentityExpress.Manager.UI.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CustomIdentityDb>(options =>
{
    options.UseSqlServer(@"Server=localhost;User Id=IISConnect;Password=Password123!;Database=CustomIdentityDbTest;Encrypt=False");
});

builder.Services.AddAdminUI(options =>
{
    options.IdentityType = IdentityType.CustomIdentity;
}).WithIdentityStore<CustomSSOStoreFactory>();

var app = builder.Build();

app.UseAdminUI();

// app.MapGet("/runseed", async (CustomIdentityDb context) =>
// {
//     var adminRole = new TenantRole
//     {
//         Description = "Admin",
//         Id = Guid.NewGuid().ToString(),
//         Name = "AdminUI Administrator",
//         NonEditable = true
//     };
//     
//     var euWestTenant = new Tenant
//     {
//         Name = "EUWest"
//     };
//     
//     var naTenant = new Tenant
//     {
//         Name = "NorthAmerica"
//     };
//
//     var tenantList = new List<Tenant> {euWestTenant, naTenant};
//
//     var adminUser = new TenantUser
//     {
//         Email = "sam@sam.com",
//         FirstName = "Sam",
//         IsDeleted = false,
//         LastName = "Jones",
//         LockoutEnabled = false,
//         TwoFactorEnabled = true,
//         UserId = Guid.NewGuid().ToString(),
//         UserName = "sam",
//         Roles = new List<TenantRole>
//         {
//             adminRole
//         },
//         ConcurrencyStamp = Guid.NewGuid().ToString(),
//         Tenant = euWestTenant
//     };
//
//     adminUser.Password = new PasswordHasher<TenantUser>().HashPassword(adminUser, "Password123!");
//     
//     if (!context.Roles.Any(role => role.Name == "AdminUI Administrator"))
//     {
//         context.Add(adminRole);
//     }
//
//     if (!context.Tenants.Any(tenant => tenant.Name == "EUWest"))
//     {
//         context.AddRange(tenantList);
//     }
//
//     if (!context.Users.Any(user => user.UserName == "sam"))
//     {
//         context.Users.Add(adminUser);
//     }
//
//     if (!context.ClaimTypes.Any(ct => ct.Name == "Tenancy"))
//     {
//         var tenantsEnumClaimType = new TenantClaimType
//         {
//             Description = "Tenants In The System",
//             Id = TenantClaimTypeConsts.TenantCTId,
//             Name = TenantClaimTypeConsts.TenantCTName,
//             DisplayName = "Tenants In System",
//             IsRequired = true,
//             IsReserved = true,
//             ValueType = SSOClaimValueType.Enum,
//             IsUserEditable = false,
//             RegularExpressionValidationRule = string.Empty,
//             RegularExpressionValidationFailureDescription = string.Empty,
//             AllowedValues = new List<TenantEnumClaimTypeValue>()
//         };
//
//         context.ClaimTypes.Add(tenantsEnumClaimType);
//     }
//     
//     context.SaveChanges();
// });

app.Run();