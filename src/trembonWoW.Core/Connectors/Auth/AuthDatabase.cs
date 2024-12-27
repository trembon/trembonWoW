using Dapper;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using trembonWoW.Core.Connectors.Auth.Records;

namespace trembonWoW.Core.Connectors.Auth;

public interface IAuthDatabase
{
    Task<Account?> GetAccount(string username);

    Task<Account?> GetAccountById(uint id);
}

public class AuthDatabase([FromKeyedServices("auth")] MySqlConnection mysqlConnection) : IAuthDatabase
{
    public async Task<Account?> GetAccount(string username)
    {
        var parameters = new { username };
        var sql = "SELECT id,username,email,salt,verifier FROM account WHERE username = UPPER(@username)";
        return await mysqlConnection.QuerySingleOrDefaultAsync<Account>(sql, parameters);
    }

    public async Task<Account?> GetAccountById(uint id)
    {
        var parameters = new { id };
        var sql = "SELECT id,username,email,salt,verifier FROM account WHERE id = @id";
        return await mysqlConnection.QuerySingleOrDefaultAsync<Account>(sql, parameters);
    }
}