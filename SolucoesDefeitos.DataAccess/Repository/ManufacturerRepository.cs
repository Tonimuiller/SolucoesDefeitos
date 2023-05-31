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
    public class ManufacturerRepository: IManufacturerRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ManufacturerRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Manufacturer> AddAsync(Manufacturer entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO manufacturer")
                .AppendLine("\t(creationdate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname)")
                .AppendLine("VALUES")
                .AppendLine("\t(@creationdate,")
                .AppendLine("\t@enabled,")
                .AppendLine("\t@name);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            entity.ManufacturerId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            return entity;
        }

        public async Task DeleteAsync(Manufacturer entity, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM manufacturer WHERE manufacturerid = @manufacturerid",
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task<PagedData<Manufacturer>> FilterAsync(CancellationToken cancellationToken, string name = null, int page = 1, int pageSize = 20)
        {
            var listViewModel = new PagedData<Manufacturer>();
            listViewModel.CurrentPage = page;
            listViewModel.PageSize = pageSize;
            var skip = (page - 1) * pageSize;
            var totalItemsQueryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tCOUNT(manufacturerId) AS TotalItems")
                .AppendLine("FROM")
                .AppendLine("\tmanufacturer");

            if (!string.IsNullOrEmpty(name))
            {
                totalItemsQueryBuilder.AppendLine("WHERE")
                    .AppendLine("\tname LIKE @name");
                name = $"{name}%";
            }

            var totalItemsParameters = new
            {
                name
            };

            var commandDefinition = new CommandDefinition(
                totalItemsQueryBuilder.ToString(),
                totalItemsParameters,
                _database.DbTransaction,
                cancellationToken: cancellationToken);

            listViewModel.TotalRecords = await _database.DbConnection.QuerySingleOrDefaultAsync<int>(commandDefinition);
            while (listViewModel.CurrentPage > 1 && skip >= listViewModel.TotalRecords) 
            {
                listViewModel.CurrentPage--;
                skip = (listViewModel.CurrentPage - 1) * pageSize;
            }

            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tmanufacturerid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname")
                .AppendLine("FROM")
                .AppendLine("\tmanufacturer");
            
            if (!string.IsNullOrEmpty(name))
            {
                queryBuilder.AppendLine("WHERE")
                    .AppendLine("\tname LIKE @name");
                name = $"{name}%";
            }

            queryBuilder.AppendLine("ORDER BY name");

            queryBuilder.AppendLine("LIMIT @pageSize OFFSET @skip");

            var parameters = new
            {
                name,
                skip,
                pageSize
            };

            commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                parameters,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            listViewModel.Data = await _database.DbConnection.QueryAsync<Manufacturer>(commandDefinition);
            return listViewModel;
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tmanufacturerid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname")
                .AppendLine("FROM")
                .AppendLine("\tmanufacturer")
                .AppendLine("ORDER BY")
                .AppendLine("\tname");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<Manufacturer>(commandDefinition);
        }

        public async Task<Manufacturer> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tmanufacturerid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tname")
                .AppendLine("FROM")
                .AppendLine("\tmanufacturer")
                .AppendLine("WHERE")
                .AppendLine("\tmanufacturerId = @manufacturerId");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { manufacturerId = keyValue },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<Manufacturer>(commandDefinition);
        }

        public async Task UpdateAsync(Manufacturer entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tmanufacturer")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tenabled = @enabled,")
                .AppendLine("\tname = @name")
                .AppendLine("WHERE")
                .AppendLine("\tmanufacturerid = @manufacturerid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
