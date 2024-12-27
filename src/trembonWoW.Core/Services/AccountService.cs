using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Auth;
using trembonWoW.Core.Connectors.Auth.Records;
using trembonWoW.Core.Connectors.RemoteAccess;

namespace trembonWoW.Core.Services;

public interface IAccountService
{
    Task<bool> ChangePassword(int id, string newPassword);

    Task<Account?> Get(string username);

    Task<Account?> GetById(int id);
}

public partial class AccountService(IAuthDatabase authDatabase, IRemoteAccessSoapAPI remoteAccessSoapAPI) : IAccountService
{
    [GeneratedRegex("^[a-zA-Z0-9!]+$")]
    private static partial Regex PasswordRegex();

    public async Task<bool> ChangePassword(int id, string newPassword)
    {
        var account = await GetById(id);
        ArgumentNullException.ThrowIfNull(account, nameof(id));

        ArgumentOutOfRangeException.ThrowIfLessThan(newPassword.Length, 4, nameof(newPassword));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newPassword.Length, 16, nameof(newPassword));

        if (!PasswordRegex().IsMatch(newPassword))
            throw new ArgumentException("Password contains invalid characters", nameof(newPassword));

        return await remoteAccessSoapAPI.ChangeAccountPassword(account.Username, newPassword);
    }

    public Task<Account?> Get(string username)
    {
        return authDatabase.GetAccount(username);
    }

    public Task<Account?> GetById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0, nameof(id));

        return authDatabase.GetAccountById((uint)id);
    }
}