using Microsoft.AspNetCore.Identity;

namespace trembonWoW.Authentication
{
    public class WoWUser : IdentityUser
    {
        public string Salt { get; set; }

        public int GMLevel { get; set; }
    }
}