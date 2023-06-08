using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Provider;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AnomalyProductSpecificationRepository : IAnomalyProductSpecificationRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AnomalyProductSpecificationRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<AnomalyProductSpecification> AddAsync(AnomalyProductSpecification entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO anomalyproductspecification")
                .AppendLine("\t(creationdate,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tproductid,")
                .AppendLine("\tmanufactureyear)")
                .AppendLine("VALUES")
                .AppendLine("\t(@creationdate,")
                .AppendLine("\t@anomalyid,")
                .AppendLine("\t@productid,")
                .AppendLine("\t@manufactureyear);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            entity.AnomalyProductSpecificationId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            return entity;
        }

        public async Task DeleteAsync(CancellationToken cancellationToken, params int[] anomalyProductSpecificationIds)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM anomalyproductspecification WHERE anomalyproductspecificationid in @anomalyproductspecificationids",
                new { anomalyProductSpecificationIds },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task DeleteAsync(AnomalyProductSpecification entity, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM anomalyproductspecification WHERE anomalyproductspecificationid = @anomalyproductspecificationid",
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task DeleteByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM anomalyproductspecification WHERE anomalyid = @anomalyid",
                new { anomalyId },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task<IEnumerable<AnomalyProductSpecification>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyproductspecificationid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tproductid,")
                .AppendLine("\tmanufactureyear")
                .AppendLine("FROM")
                .AppendLine("\tanomalyproductspecification");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<AnomalyProductSpecification>(commandDefinition);
        }

        public async Task<IEnumerable<int>> GetAnomalyProductSpecificationIdsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyproductspecificationid")
                .AppendLine("FROM")
                .AppendLine("\tanomalyproductspecification")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyid = @anomalyid")
                ;
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { anomalyId },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<int>(commandDefinition);
        }

        public async Task<AnomalyProductSpecification> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tanomalyproductspecificationid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tproductid,")
                .AppendLine("\tmanufactureyear")
                .AppendLine("FROM")
                .AppendLine("\tanomalyproductspecification")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyproductspecificationid = @anomalyproductspecificationid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { anomalyproductspecificationid  = keyValue },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<AnomalyProductSpecification>(commandDefinition);
        }

        public async Task<IEnumerable<AnomalyProductSpecification>> GetProductsSpecificationsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\taps.anomalyproductspecificationid,")
                .AppendLine("\taps.creationdate,")
                .AppendLine("\taps.updatedate,")
                .AppendLine("\taps.anomalyid,")
                .AppendLine("\taps.manufactureyear,")
                .AppendLine("\tp.productid,")
                .AppendLine("\tp.creationdate,")
                .AppendLine("\tp.updatedate,")
                .AppendLine("\tp.enabled,")
                .AppendLine("\tp.name,")
                .AppendLine("\tp.code,")
                .AppendLine("\tm.manufacturerid,")
                .AppendLine("\tm.creationdate,")
                .AppendLine("\tm.updatedate,")
                .AppendLine("\tm.enabled,")
                .AppendLine("\tm.name,")
                .AppendLine("\tg.productgroupid,")
                .AppendLine("\tg.creationdate,")
                .AppendLine("\tg.updatedate,")
                .AppendLine("\tg.enabled,")
                .AppendLine("\tg.description")
                .AppendLine("FROM")
                .AppendLine("\tanomalyproductspecification aps")
                .AppendLine("INNER JOIN product p ON aps.productid = p.productid")
                .AppendLine("LEFT JOIN manufacturer m ON p.manufacturerid = m.manufacturerid")
                .AppendLine("LEFT JOIN productgroup g ON p.productgroupid = g.productgroupid")
                .AppendLine("WHERE")
                .AppendLine("\taps.anomalyid = @anomalyid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                new { anomalyId },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<AnomalyProductSpecification, Product, Manufacturer, ProductGroup, AnomalyProductSpecification>(
                commandDefinition,
                (anomalyProductSpecification, product, manufacturer, productGroup) =>
                {
                    product.ProductGroupId = productGroup.ProductGroupId;
                    product.ProductGroup = productGroup;
                    product.ManufacturerId = manufacturer.ManufacturerId;
                    product.Manufacturer = manufacturer;
                    anomalyProductSpecification.ProductId = product.ProductId;
                    anomalyProductSpecification.Product = product;
                    return anomalyProductSpecification;
                },
                "productid, manufacturerid, productgroupid");
        }

        public async Task UpdateAsync(AnomalyProductSpecification entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tanomalyproductspecification")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tanomalyid = @anomalyid,")
                .AppendLine("\tproductid = @productid,")
                .AppendLine("\tmanufactureyear = @manufactureyear")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyproductspecificationid - @anomalyproductspecificationid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
