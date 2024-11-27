using Microsoft.AspNetCore.Identity;

namespace trembonWoW.Authentication
{
    public class WoWPasswordHasher : IPasswordHasher<WoWUser>
    {
        public string HashPassword(WoWUser user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(WoWUser user, string hashedPassword, string providedPassword)
        {
            return PasswordVerificationResult.Success;
        }
    }
}