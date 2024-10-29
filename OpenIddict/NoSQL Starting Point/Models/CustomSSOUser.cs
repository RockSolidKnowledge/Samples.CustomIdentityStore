using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rsk.CustomIdentity.Interfaces;

namespace NoSQLStartingPoint.Models;

public class CustomSSOUser : ISSOUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string ConcurrencyStamp { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; }
    public bool IsDeleted { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; }

    [BsonIgnoreIfNull] 
    public ICollection<ISSORole> Roles { get; set; } = new List<ISSORole>();

    [BsonIgnoreIfNull]
    public ICollection<ISSOClaim> Claims { get; set; } = new List<ISSOClaim>();
}