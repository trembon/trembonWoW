using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Authentication;

namespace trembonWoW.Core.Authentication
{
    public class WoWPasswordHasher : IPasswordHasher<WoWUser>
    {
        public string HashPassword(WoWUser user, string password)
        {
            ArgumentNullException.ThrowIfNull(user.UserName);

            byte[] hashedPassword = CalculateSRP6Verifier(user.UserName, password, user.Salt);
            return Encoding.UTF8.GetString(hashedPassword);
        }

        public PasswordVerificationResult VerifyHashedPassword(WoWUser user, string hashedPassword, string providedPassword)
        {
            ArgumentNullException.ThrowIfNull(user.UserName);

            byte[] checkVerifier = CalculateSRP6Verifier(user.UserName, providedPassword, user.Salt);

            return user.Verifier.SequenceEqual(checkVerifier) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        private static readonly BigInteger g = 7;
        private static readonly BigInteger N = BigInteger.Parse("00894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7", NumberStyles.AllowHexSpecifier);

        public static byte[] CalculateSRP6Verifier(string username, string password, byte[] salt_bytes)
        {
            SHA1 sha1 = SHA1.Create();

            // calculate first hash
            byte[] login_bytes = Encoding.UTF8.GetBytes((username + ':' + password).ToUpper());
            byte[] h1_bytes = sha1.ComputeHash(login_bytes);

            // calculate second hash
            byte[] h2_bytes = sha1.ComputeHash(salt_bytes.Concat(h1_bytes).ToArray());

            // convert to integer (little-endian)
            BigInteger h2 = new BigInteger(h2_bytes, true);

            // g^h2 mod N
            BigInteger verifier = BigInteger.ModPow(g, h2, N);

            // convert back to a byte array (little-endian)
            byte[] verifier_bytes = verifier.ToByteArray();

            // pad to 32 bytes, remember that zeros go on the end in little-endian!
            byte[] verifier_bytes_padded = new byte[Math.Max(32, verifier_bytes.Length)];
            Buffer.BlockCopy(verifier_bytes, 0, verifier_bytes_padded, 0, verifier_bytes.Length);

            // done!
            return verifier_bytes_padded;
        }
    }
}