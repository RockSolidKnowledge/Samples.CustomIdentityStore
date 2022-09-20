using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint.Models;

public class CustomSSOClaimType : ISSOClaimType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }
    public bool IsReserved { get; set; }
    public SSOClaimValueType ValueType { get; set; }
    public string RegularExpressionValidationRule { get; set; }
    public string RegularExpressionValidationFailureDescription { get; set; }
    public bool IsUserEditable { get; set; }
    public ICollection<string> AllowedValues { get; set; }
}
