using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Dto.Anomaly.Request;
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
    public class AnomalyRepository : IAnomalyRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AnomalyRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Anomaly> AddAsync(Anomaly entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO anomaly")
                .AppendLine("\t(creationdate,")
                .AppendLine("\tsummary,")
                .AppendLine("\tdescription,")
                .AppendLine("\trepairsteps)")
                .AppendLine("VALUES")
                .AppendLine("\t(@creationdate,")
                .AppendLine("\t@summary,")
                .AppendLine("\t@description,")
                .AppendLine("\t@repairsteps);")
                .AppendLine("SELECT LAST_INSERT_ID();");

            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            entity.AnomalyId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            return entity;
        }

        public async Task<ResponseDto> DeleteAsync(Anomaly entity, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM anomaly WHERE anomalyid = @anomalyid",
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

        public async Task<PagedData<Anomaly>> FilterAsync(AnomalyFilterRequest request, CancellationToken cancellationToken)
        {
            var listViewModel = new PagedData<Anomaly>();
            listViewModel.CurrentPage = request.Page;
            listViewModel.PageSize = request.PageSize;
            var skip = (request.Page - 1) * request.PageSize;
            var totalItemsQueryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tCOUNT(anomalyId) AS TotalItems")
                .AppendLine("FROM")
                .AppendLine("\tanomaly")
                .AppendLine(BuildFilterWhere(request));

            var commandDefinition = new CommandDefinition(
                totalItemsQueryBuilder.ToString(),
                new
                {
                    searchTerm = $"%{request.SearchTerm?.ToLower()}%",
                    request.ManufacturerIds,
                    request.ProductGroupIds,
                    request.ProductIds
                },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            listViewModel.TotalRecords = await _database.DbConnection.QuerySingleOrDefaultAsync<int>(commandDefinition);
            while (listViewModel.CurrentPage > 1 && skip >= listViewModel.TotalRecords)
            {
                listViewModel.CurrentPage--;
                skip = (listViewModel.CurrentPage - 1) * request.PageSize;
            }

            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tsummary,")
                .AppendLine("\tdescription,")
                .AppendLine("\trepairsteps")
                .AppendLine("FROM")
                .AppendLine("\tanomaly")
                .AppendLine(BuildFilterWhere(request));

            queryBuilder.AppendLine("ORDER BY creationdate DESC");
            queryBuilder.AppendLine("LIMIT @pageSize OFFSET @skip");

            var parameters = new
            {
                searchTerm = $"%{request.SearchTerm?.ToLower()}%",
                request.ManufacturerIds,
                request.ProductGroupIds,
                request.ProductIds,
                skip,                
                request.PageSize
            };

            commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                parameters,
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            listViewModel.Data = await _database.DbConnection.QueryAsync<Anomaly>(commandDefinition);
            return listViewModel;
        }

        public async Task<IEnumerable<Anomaly>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tsummary,")
                .AppendLine("\tdescription,")
                .AppendLine("\trepairsteps")
                .AppendLine("FROM")
                .AppendLine("\tanomaly");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<Anomaly>(commandDefinition);
        }

        public async Task<Anomaly> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tsummary,")
                .AppendLine("\tdescription,")
                .AppendLine("\trepairsteps")
                .AppendLine("FROM")
                .AppendLine("\tanomaly")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyid = @anomalyid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { anomalyId = keyValue },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<Anomaly>(commandDefinition);
        }

        public async Task UpdateAsync(Anomaly entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tanomaly")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tsummary = @summary,")
                .AppendLine("\tdescription = @description,")
                .AppendLine("\trepairsteps = @repairsteps")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyid = @anomalyid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        private string BuildFilterWhere(AnomalyFilterRequest request)
        {
            if (!request.Filtered)
                return string.Empty;
            
            
            var queryBuilder = new StringBuilder()
                .AppendLine("WHERE");
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                queryBuilder
                    .AppendLine("\t(")
                    .AppendLine("\t\t(LOWER(summary) LIKE @searchTerm)")
                    .AppendLine("\t\t OR (LOWER(description) LIKE @searchTerm)")
                    .AppendLine("\t\t OR (LOWER(repairsteps) LIKE @searchTerm)")
                    .AppendLine("\t)");
            }

            if (!string.IsNullOrEmpty(request.SearchTerm)
                && ((request.ProductIds?.Any() ?? false)
                || (request.ProductGroupIds?.Any() ?? false)
                || (request.ManufacturerIds?.Any() ?? false)))
            {
                queryBuilder.AppendLine("\tAND");
            }

            if (request.ProductIds?.Any() ?? false)
            {
                queryBuilder
                    .AppendLine("\tanomalyid IN")
                    .AppendLine("\t(")
                    .AppendLine("\t\tSELECT")
                    .AppendLine("\t\t\tanomalyid")
                    .AppendLine("\t\tFROM")
                    .AppendLine("\t\t\tanomalyproductspecification")
                    .AppendLine("\t\tWHERE")
                    .AppendLine("\t\t\tproductid in @productIds")
                    .AppendLine("\t)");
            }
            else if (request.ProductGroupIds?.Any() ?? false)
            {
                queryBuilder
                    .AppendLine("\tanomalyid IN")
                    .AppendLine("\t(")
                    .AppendLine("\t\tSELECT")
                    .AppendLine("\t\t\ti.anomalyid")
                    .AppendLine("\t\tFROM")
                    .AppendLine("\t\t\tanomalyproductspecification i")
                    .AppendLine("\t\t\tINNER JOIN product p ON i.productid = p.productid")
                    .AppendLine("\t\tWHERE")
                    .AppendLine("\t\t\tp.productgroupid in @productGroupIds")
                    .AppendLine("\t)");
            }
            else if (request.ManufacturerIds?.Any() ?? false)
            {
                queryBuilder
                    .AppendLine("\tanomalyid IN")
                    .AppendLine("\t(")
                    .AppendLine("\t\tSELECT")
                    .AppendLine("\t\t\ti.anomalyid")
                    .AppendLine("\t\tFROM")
                    .AppendLine("\t\t\tanomalyproductspecification i")
                    .AppendLine("\t\t\tINNER JOIN product p ON i.productid = p.productid")
                    .AppendLine("\t\tWHERE")
                    .AppendLine("\t\t\tp.manufacturerid in @manufacturerIds")
                    .AppendLine("\t)");
            }

            return queryBuilder.ToString();
        }
    }
}
