﻿using Rsk.CustomIdentity.Interfaces;

namespace EF_Tenancy.Models;

public class CustomSSOUser : ISSOUser
{
    public string Id { get; init; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ConcurrencyStamp { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsBlocked { get; }
    public bool IsDeleted { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; }
    public ICollection<ISSORole> Roles { get; set; } = new List<ISSORole>();
    public ICollection<ISSOClaim> Claims { get; set; } = new List<ISSOClaim>();
}