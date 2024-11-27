using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Auth.Models;

namespace trembonWoW.Core.Connectors.Auth;

public interface IAuthDatabase
{
    Task<bool> AuthenticateAccount(string username, string password);
}

public class AuthDatabase(MySqlConnection mysqlConnection) : IAuthDatabase
{
    public async Task<bool> AuthenticateAccount(string username, string password)
    {
        var parameters = new { username, password };
        var sql = "select username,salt,verifier from account where username = upper(@username)";
        var result = await mysqlConnection.QuerySingleAsync<AccountAuthenticationRecord>(sql, parameters);
        if (result == null)
            return false;

        bool verified = VerifySRP6Login(result.Username, password, result.Salt, result.Verifier);

        return verified;
    }

    // algorithm constants
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

    public static bool VerifySRP6Login(string username, string password, byte[] salt, byte[] verifier)
    {
        // re-calculate the verifier using the provided username + password and the stored salt
        byte[] checkVerifier = CalculateSRP6Verifier(username, password, salt);

        // compare it against the stored verifier
        return verifier.SequenceEqual(checkVerifier);
    }

    /*private byte[] CalculateSRP6Verifier(string username, string password, byte[] salt)
    {
        // algorithm constants
        BigInteger g = new(7);
        BigInteger N = BigInteger.Parse("894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7", System.Globalization.NumberStyles.HexNumber);

        // calculate first hash
        byte[] accountData = Encoding.UTF8.GetBytes($"{username}:{password}".ToUpperInvariant());
        byte[] firstHash = SHA1.HashData(accountData);

        string temp1 = Encoding.UTF8.GetString(firstHash);

        // calculate second hash
        byte[] hashWithSalt = Combine(salt, firstHash);
        byte[] secondHash = SHA1.HashData(hashWithSalt);

        string temp2 = Encoding.UTF8.GetString(secondHash);

        // convert to integer (little-endian)
        BigInteger h2Int = new(secondHash.Reverse().ToArray());

        // g^h2 mod N
        BigInteger verifier = BigInteger.ModPow(g, h2Int, N);

        // convert back to a byte array (little-endian)
        byte[] verifierBytes = verifier.ToByteArray();

        // pad to 32 bytes, remember that zeros go on the end in little-endian!
        Array.Resize(ref verifierBytes, 32);

        return verifierBytes;
    }

    private byte[] Combine(byte[] first, byte[] second)
    {
        byte[] result = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, result, 0, first.Length);
        Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
        return result;
    }*/
}