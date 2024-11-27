using Dapper;
using MySqlConnector;
using trembonWoW.Core.Connectors.Auth.Models;

namespace trembonWoW.Core.Connectors.Auth;

public interface IAuthDatabase
{
    Task<Account?> GetAccount(string username);

    Task<Account?> GetAccountById(string id);
}

public class AuthDatabase(MySqlConnection mysqlConnection) : IAuthDatabase
{
    public async Task<Account?> GetAccount(string username)
    {
        var parameters = new { username };
        var sql = "SELECT id,username,email,salt,verifier FROM account WHERE username = UPPER(@username)";
        return await mysqlConnection.QuerySingleAsync<Account>(sql, parameters);
    }

    public async Task<Account?> GetAccountById(string id)
    {
        var parameters = new { id };
        var sql = "SELECT id,username,email,salt,verifier FROM account WHERE id = @id";
        return await mysqlConnection.QuerySingleAsync<Account>(sql, parameters);
    }
}