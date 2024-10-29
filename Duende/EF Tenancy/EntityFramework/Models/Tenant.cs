using System.ComponentModel.DataAnnotations;

namespace EF_Tenancy.EntityFramework.Models;

public class Tenant
{
    [Key]    
    public string Name { get; set; }
}