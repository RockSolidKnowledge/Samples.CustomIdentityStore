using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rsk.CustomIdentity.Interfaces;

public class CustomSSOClaim : ISSOClaim
{
    [BsonId]
    public int Id { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}