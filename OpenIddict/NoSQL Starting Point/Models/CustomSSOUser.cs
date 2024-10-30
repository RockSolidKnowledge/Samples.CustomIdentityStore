using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint.Models;

public class CustomSSOUser : ISSOUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("UserName")]
    public string UserName { get; set; }

    [BsonElement("Password")]
    public string Password { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("ConcurrencyStamp")]
    public string ConcurrencyStamp { get; set; }

    [BsonElement("TwoFactorEnabled")]
    public bool TwoFactorEnabled { get; set; }

    [BsonElement("FirstName")]
    public string FirstName { get; set; }

    [BsonElement("LastName")]
    public string LastName { get; set; }

    [BsonElement("IsBlocked")]
    public bool IsBlocked { get; set; }

    [BsonElement("IsDeleted")]
    public bool IsDeleted { get; set; }

    [BsonElement("LockoutEnabled")]
    public bool LockoutEnabled { get; set; }

    [BsonElement("LockoutEnd")]
    public DateTimeOffset? LockoutEnd { get; set; }

    [BsonIgnoreIfNull] 
    public ICollection<CustomSSORole> Roles { get; set; } = new List<CustomSSORole>();

    [BsonIgnoreIfNull]
    public ICollection<ISSOClaim> Claims { get; set; } = new List<ISSOClaim>();
}