using Dapper;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Characters.Records;

namespace trembonWoW.Core.Connectors.Characters
{
    public interface ICharactersDatabase
    {
        Task<IEnumerable<Character>> GetCharacters(uint accountId);
    }

    public class CharactersDatabase([FromKeyedServices("characters")] MySqlConnection mysqlConnection) : ICharactersDatabase
    {
        public async Task<IEnumerable<Character>> GetCharacters(uint accountId)
        {
            var parameters = new { accountId };
            var sql = "SELECT guid,account,name,level,race,class FROM characters WHERE account = @accountId";
            return await mysqlConnection.QueryAsync<Character>(sql, parameters);
        }
    }
}