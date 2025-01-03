﻿using Microsoft.AspNetCore.Identity;
using System.Text;
using trembonWoW.Core.Connectors.Auth.Records;

namespace trembonWoW.Core.Authentication
{
    public class WoWUser : IdentityUser
    {
        public byte[] Salt { get; private set; }
        public byte[] Verifier { get; private set; }

        public WoWUser(Account account)
        {
            Id = account.Id.ToString();
            UserName = account.Username.ToLowerInvariant();
            NormalizedUserName = account.Username.ToUpperInvariant();
            Email = account.Email?.ToLowerInvariant();
            NormalizedEmail = account.Email?.ToUpperInvariant();
            Salt = account.Salt;
            PasswordHash = Encoding.UTF8.GetString(account.Verifier);
            Verifier = account.Verifier;
        }
    }
}