using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ProductGroupRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ProductGroup> AddAsync(ProductGroup entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO productgroup")
                .AppendLine("\t(creationdate, ")
                .AppendLine("\tenabled, ")
                .AppendLine("\tdescription, ")
                .AppendLine("\tfatherproductgroupid)")
                .AppendLine("VALUES")
                .AppendLine("\t(@creationdate,")
                .AppendLine("\t@Enabled,")
                .AppendLine("\t@Description,")
                .AppendLine("\t@FatherProductGroupId);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            entity.ProductGroupId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            return entity;
        }

        public async Task<ResponseDto> DeleteAsync(ProductGroup entity, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM productgroup WHERE productgroupid = @productgroupid",
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            try
            {
                await _database.DbConnection.ExecuteAsync(commandDefinition);
                return new ResponseDto(true);
            }
            catch (Exception ex)
            {
                return new ResponseDto(false, ex.Message);
            }
        }

        public async Task<PagedData<ProductGroup>> FilterAsync(CancellationToken cancellationToken, string description = null, int page = 1, int pageSize = 20)
        {
            var listViewModel = new PagedData<ProductGroup>();
            listViewModel.CurrentPage = page;
            listViewModel.PageSize = pageSize;
            var skip = (page - 1) * pageSize;
            var totalItemsQueryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tCOUNT(productgroupid) AS TotalItems")
                .AppendLine("FROM")
            .AppendLine("\tproductGroup");

            if (!string.IsNullOrEmpty(description))
            {
                totalItemsQueryBuilder.AppendLine("WHERE")
                    .AppendLine("\tdescription LIKE @description");
                description = $"{description}%";
            }

            var totalItemsParameters = new
            {
                description
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
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tdescription")
                .AppendLine("FROM")
            .AppendLine("\tproductgroup");

            if (!string.IsNullOrEmpty(description))
            {
                queryBuilder.AppendLine("WHERE")
                    .AppendLine("\tdescription LIKE @description");
                description = $"{description}%";
            }

            queryBuilder.AppendLine("ORDER BY description");

            queryBuilder.AppendLine("LIMIT @pageSize OFFSET @skip");

            var parameters = new
            {
                description,
                skip,
                pageSize
            };

            commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                parameters,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            listViewModel.Data = await _database.DbConnection.QueryAsync<ProductGroup>(commandDefinition);
            return listViewModel;
        }

        public async Task<IEnumerable<ProductGroup>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tdescription,")
                .AppendLine("\tfatherproductgroupid")
                .AppendLine("FROM")
                .AppendLine("\tproductgroup")
                .AppendLine("ORDER BY")
                .AppendLine("\tdescription");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<ProductGroup>(commandDefinition);
        }

        public async Task<ProductGroup> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tdescription,")
                .AppendLine("\tfatherproductgroupid")
                .AppendLine("FROM")
                .AppendLine("\tproductgroup")
                .AppendLine("WHERE")
                .AppendLine("\tproductgroupid = @productgroupid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { productGroupId =  keyValue },
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<ProductGroup>(commandDefinition);
        }

        public async Task LoadSubgroupsAsync(ProductGroup productGroup, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tdescription,")
                .AppendLine("\tfatherproductgroupid")
                .AppendLine("FROM")
                .AppendLine("\tproductgroup")
                .AppendLine("WHERE")
                .AppendLine("\tfatherproductgroupid = @productgroupid")
                .AppendLine("ORDER BY")
                .AppendLine("\tdescription");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                productGroup,
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            productGroup.Subgroups = (await _database.DbConnection.QueryAsync<ProductGroup>(commandDefinition)).ToList();
        }

        public async Task UpdateAsync(ProductGroup entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tproductgroup")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tenabled = @enabled,")
                .AppendLine("\tdescription = @description,")
                .AppendLine("\tfatherproductgroupid = @fatherproductgroupid")
                .AppendLine("WHERE")
                .AppendLine("\tproductgroupid = @productgroupid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
