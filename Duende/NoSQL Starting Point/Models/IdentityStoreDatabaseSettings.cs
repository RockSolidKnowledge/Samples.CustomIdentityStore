namespace NoSQLStartingPoint.Models;

public class IdentityStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
    
    public string RolesCollectionName { get; set; } = null!;
    
    public string ClaimTypesCollectionName { get; set; } = null!;
}