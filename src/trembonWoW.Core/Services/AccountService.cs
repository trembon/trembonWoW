using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Auth;
using trembonWoW.Core.Connectors.Auth.Records;

namespace trembonWoW.Core.Services;

public interface IAccountService
{
    Task<Account?> Get(string username);

    Task<Account?> GetById(string id);
}

public class AccountService(IAuthDatabase authDatabase) : IAccountService
{
    public Task<Account?> Get(string username)
    {
        return authDatabase.GetAccount(username);
    }

    public Task<Account?> GetById(string id)
    {
        return authDatabase.GetAccountById(id);
    }
}