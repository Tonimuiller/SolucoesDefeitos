using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class ManufacturerRepository :
        BaseRepository<Manufacturer, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IManufacturerRepository,
        IRepository<Manufacturer>
    {
        public ManufacturerRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) 
            : base(unitOfWork)
        {
        }

        public async Task<ListViewModel<Manufacturer>> FilterAsync(CancellationToken cancellationToken, string name = null, int page = 1, int pageSize = 20)
        {
            var listViewModel = new ListViewModel<Manufacturer>();
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

            listViewModel.TotalRecords = await this.QuerySingleRawAsync<int>(totalItemsQueryBuilder.ToString(), totalItemsParameters, cancellationToken);
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

            listViewModel.Data = await this.QueryRawAsync(queryBuilder.ToString(), parameters, cancellationToken);
            return listViewModel;
        }
    }
}
