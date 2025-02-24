using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSQLStartingPoint.Models;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint.Stores;

public class ClaimTypeStore : ISSOClaimTypeStore
{
    private readonly IMongoCollection<CustomSSOClaimType> dbClaimTypes;

    public ClaimTypeStore(IOptions<IdentityStoreDatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.DatabaseName);

        dbClaimTypes = mongoDatabase.GetCollection<CustomSSOClaimType>(
            dbSettings.Value.ClaimTypesCollectionName);
    }
    
    public async Task<ISSOClaimType> CreateClaimType(ISSOClaimType claimType)
    {
        var newClaimType = new CustomSSOClaimType
        {
            Name = claimType.Name,
            DisplayName = claimType.DisplayName,
            Description = claimType.Description,
            IsRequired = claimType.IsRequired,
            IsReserved = claimType.IsReserved,
            ValueType = claimType.ValueType,
            RegularExpressionValidationRule = claimType.RegularExpressionValidationRule,
            RegularExpressionValidationFailureDescription = claimType.RegularExpressionValidationFailureDescription,
            IsUserEditable = claimType.IsUserEditable,
            AllowedValues = claimType.AllowedValues
        };
        
        await dbClaimTypes.InsertOneAsync(newClaimType);

        var claimTypeQuery = await dbClaimTypes.FindAsync(ct => ct.Name == claimType.Name);
        return await claimTypeQuery.FirstOrDefaultAsync();
    }

    public async Task DeleteClaimType(ISSOClaimType claimType)
    {
        await dbClaimTypes.FindOneAndDeleteAsync(ct => ct.Id == claimType.Id);
    }

    public async Task<IEnumerable<ISSOClaimType>> GetAllClaimTypes()
    {
        var allClaims = await dbClaimTypes.FindAsync(_ => true);
        return allClaims.ToEnumerable();
    }

    public async Task<ISSOClaimType> UpdateClaimType(ISSOClaimType claimType)
    {
        var mappedClaimType = MapClaimType(claimType);

        await dbClaimTypes.FindOneAndReplaceAsync(ct => ct.Id == mappedClaimType.Id, mappedClaimType);

        var claimTypeQuery = await dbClaimTypes.FindAsync(ct => ct.Id == mappedClaimType.Id);
        return await claimTypeQuery.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ISSOClaimType>> BulkUpdateClaimTypes(IEnumerable<ISSOClaimType> claimTypesToUpdate)
    {
        foreach (var claimType in claimTypesToUpdate)
        {
            var updatedClaimType = MapClaimType(claimType);
            await dbClaimTypes.FindOneAndReplaceAsync(ct => ct.Id == claimType.Id, updatedClaimType);
        }

        var existingClaimTypesQuery = await dbClaimTypes.FindAsync(_ => true);
        return await existingClaimTypesQuery.ToListAsync();
    }

    public async Task<IEnumerable<ISSOClaimType>> BulkCreateClaimTypes(IEnumerable<ISSOClaimType> claimTypesToAdd)
    {
        foreach (var claimType in claimTypesToAdd)
        {
            await dbClaimTypes.InsertOneAsync(MapClaimType(claimType));
        }

        var existingClaimTypesQuery = await dbClaimTypes.FindAsync(_ => true);
        return await existingClaimTypesQuery.ToListAsync();
    }

    private CustomSSOClaimType MapClaimType(ISSOClaimType claimType)
    {
        return new CustomSSOClaimType
        {
            Name = claimType.Name,
            DisplayName = claimType.DisplayName,
            Description = claimType.Description,
            IsRequired = claimType.IsRequired,
            IsReserved = claimType.IsReserved,
            ValueType = claimType.ValueType,
            RegularExpressionValidationRule = claimType.RegularExpressionValidationRule,
            RegularExpressionValidationFailureDescription = claimType.RegularExpressionValidationFailureDescription,
            IsUserEditable = claimType.IsUserEditable,
            AllowedValues = claimType.AllowedValues
        };
    }
}