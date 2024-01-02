using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Provider;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<User> AddAsync(User entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .Append("INSERT INTO")
                .AppendLine("\tuser ")
                .Append("(creationdate, ")
                .Append("enabled, ")
                .Append("name, ")
                .Append("login, ")
                .Append("email, ")
                .Append("password)")
                .AppendLine("VALUES ")
                .Append("(@creationdate, ")
                .Append("@enabled, ")
                .Append("@name, ")
                .Append("@login, ")
                .Append("@email, ")
                .Append("@password);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            var entityId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            entity.UserId = entityId;
            return entity;
        }
        
        public async Task<ResponseDto> DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            var sql = "DELETE FROM user WHERE userid = @userid";
            var commandDefinition = new CommandDefinition(
                sql,
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
            return new ResponseDto(true);
        }

        public async Task<bool> ExistsAsync(int userId, CancellationToken cancellationToken)
        {
            var sql = "SELECT COUNT(userid) AS RecordCount FROM  user WHERE userid = @userid";
            var commandDefinition = new CommandDefinition(
                sql,
                new { userId },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition) > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\tuserid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname,")
                .AppendLine("\tlogin,")
                .AppendLine("\tpassword,")
                .AppendLine("\temail")
                .AppendLine("FROM")
                .AppendLine("\tuser");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<User>(commandDefinition);
        }

        public async Task<User> GetByCredentialsAsync(string loginOrEmail, string password, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\tuserid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname,")
                .AppendLine("\tlogin,")
                .AppendLine("\tpassword,")
                .AppendLine("\temail")
                .AppendLine("FROM")
                .AppendLine("\tuser")
                .AppendLine("WHERE")
                .AppendLine("\t(login = @loginOrEmail OR email = @loginOrEmail)")
                .AppendLine("\tAND password = @password");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { loginOrEmail, password },
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleAsync<User>(commandDefinition);
        }

        public async Task<User> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\tuserid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname,")
                .AppendLine("\tlogin,")
                .AppendLine("\tpassword,")
                .AppendLine("\temail")
                .AppendLine("FROM")
                .AppendLine("\tuser")
                .AppendLine("WHERE")
                .AppendLine("\tuserid = @userid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { userid = keyValue },
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleAsync<User>(commandDefinition);
        }

        public async Task<bool> IsEmailAvailableAsync(string email, int? userId, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\tCOUNT(userId) AS RecordCount,")
                .AppendLine("FROM")
                .AppendLine("\tuser")
                .AppendLine("WHERE")
                .AppendLine("\temail = @email");
            if (userId.HasValue)
            {
                sqlBuilder.AppendLine("\tAND userId <> @userId");
            }

            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { userId, email },
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleAsync<int>(commandDefinition) > 0;
        }

        public async Task<bool> IsLoginAvailableAsync(string login, int? userId, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\tCOUNT(userId) AS RecordCount,")
                .AppendLine("FROM")
                .AppendLine("\tuser")
                .AppendLine("WHERE")
                .AppendLine("\tlogin = @login");
            if (userId.HasValue)
            {
                sqlBuilder.AppendLine("\tAND userId <> @userId");
            }

            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { userId, login },
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleAsync<int>(commandDefinition) > 0;
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .Append("UPDATE")
                .Append(" user")
                .AppendLine("SET")
                .Append("\tupdatedate = @updatedate,")
                .Append("\tenabled = @enabled,")
                .Append("\tname = @name,")
                .Append("\tlogin = @login,")
                .Append("\tpassword = @password,")
                .Append("\temail = @email")
                .AppendLine("WHERE")
                .AppendLine("\t userid = @userid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
