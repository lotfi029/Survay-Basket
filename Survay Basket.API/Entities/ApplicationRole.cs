using Microsoft.AspNetCore.Components.Web;

namespace Survay_Basket.API.Entities;

public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
