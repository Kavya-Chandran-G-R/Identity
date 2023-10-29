using Microsoft.AspNetCore.Identity;

namespace MyLogin.Domain.IdentityEntities
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

    }
}
