using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint.Models;

public class CustomSSORole : ISSORole
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Description { get; set; }
    public bool NonEditable { get; set; }
    public string Name { get; set; }
    public ICollection<ISSOClaim> Claims { get; set; }
}