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
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ProductRepository(
            IDateTimeProvider dateTimeProvider, 
            IDatabase database)
        {
            _dateTimeProvider = dateTimeProvider;
            _database = database;
        }

        public async Task<Product> AddAsync(Product entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO")
                .AppendLine("\tproduct ")
                .Append("(creationdate, ")
                .Append("enabled, ")
                .Append("manufacturerid, ")
                .Append("productgroupid, ")
                .Append("name, ")
                .Append("code)")
                .AppendLine("VALUES ")
                .Append("(@creationdate, ")
                .Append("@enabled, ")
                .Append("@manufacturerid, ")
                .Append("@productgroupid, ")
                .Append("@name, ")
                .Append("@code);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            var entityId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            entity.ProductId = entityId;
            return entity;
        }

        public async Task DeleteAsync(Product entity, CancellationToken cancellationToken)
        {
            var sql = "DELETE FROM product WHERE productid = @productid";
            var commandDefinition = new CommandDefinition(
                sql,
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tmanufacturerid,")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tname,")
                .AppendLine("\tcode")
                .AppendLine("FROM")
                .AppendLine("\tproduct");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                transaction: _database.DbTransaction, 
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<Product>(commandDefinition);
        }

        public async Task<Product> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var sqlBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tproductid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tenabled,")
                .AppendLine("\tmanufacturerid,")
                .AppendLine("\tproductgroupid,")
                .AppendLine("\tname,")
                .AppendLine("\tcode")
                .AppendLine("FROM")
                .AppendLine("\tproduct")
                .AppendLine("WHERE")
                .AppendLine("\tproductid = @productid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                parameters: new { productId = keyValue},
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<Product>(commandDefinition);
        }

        public async Task<IEnumerable<Product>> SearchByTermAsync(CancellationToken cancellationToken, string term)
        {
            StringBuilder queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tp.productid,")
                .AppendLine("\tp.productgroupid,")
                .AppendLine("\tp.manufacturerid,")
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
                .AppendLine("\tg.fatherproductgroupid,")
                .AppendLine("\tg.description")
                .AppendLine("FROM")
                .AppendLine("\tproduct p")
                .AppendLine("LEFT JOIN manufacturer m")
                .AppendLine("\tON p.manufacturerid = m.manufacturerid")
                .AppendLine("LEFT JOIN productgroup g")
                .AppendLine("\tON p.productgroupid = g.productgroupid")
                .AppendLine("WHERE")
                .AppendLine("\tp.enabled = 1")
                .AppendLine("\tAND (p.name LIKE @term OR m.name LIKE @term)")
                .AppendLine("\tAND m.enabled = 1")
                .AppendLine("\tAND g.enabled = 1")
                .AppendLine("ORDER BY")
                .AppendLine("\tp.name");
            var commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                new { term = $"%{term}%" },
                _database.DbTransaction,
                cancellationToken: cancellationToken);

            return await _database.DbConnection.QueryAsync<Product, Manufacturer, ProductGroup, Product>(
                commandDefinition,
                (product, manufacturer, productGroup) =>
                {
                    product.Manufacturer = manufacturer;
                    product.ProductGroup = productGroup;
                    return product;
                },
                "manufacturerid, productgroupid");
        }

        public async Task UpdateAsync(Product entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tproduct")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tenabled = @enabled,")
                .AppendLine("\tmanufacturerid = @manufacturerid,")
                .AppendLine("\tproductgroupid = @productgroupid,")
                .AppendLine("\tname = @name,")
                .AppendLine("\tcode = @code")
                .AppendLine("WHERE")
                .AppendLine("\tproductid = @productid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
