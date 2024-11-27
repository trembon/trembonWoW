using Microsoft.AspNetCore.Identity;

namespace trembonWoW.Authentication
{
    public class WoWPasswordValidator : IPasswordValidator<WoWUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<WoWUser> manager, WoWUser user, string? password)
        {
            throw new NotImplementedException();
        }
    }
}