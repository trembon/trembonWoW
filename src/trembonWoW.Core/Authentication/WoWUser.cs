using Microsoft.AspNetCore.Identity;
using System.Text;
using trembonWoW.Core.Connectors.Auth.Models;

namespace trembonWoW.Core.Authentication
{
    public class WoWUser : IdentityUser
    {
        public byte[] Salt { get; set; }

        public WoWUser(Account account)
        {
            Id = account.Id;
            UserName = account.Username.ToLowerInvariant();
            NormalizedUserName = account.Username.ToUpperInvariant();
            Email = account.Email?.ToLowerInvariant();
            NormalizedEmail = account.Email?.ToUpperInvariant();
            Salt = account.Salt;
            PasswordHash = Encoding.UTF8.GetString(account.Verifier);
        }
    }
}