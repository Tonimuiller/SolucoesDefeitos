using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AnomalyRepository :
        BaseRepository<Anomaly, DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>>,
        IRepository<Anomaly>,
        IAnomalyRepository
    {
        public AnomalyRepository(DapperUnitOfWork<SolucaoDefeitoMySqlDatabase> unitOfWork) 
            : base(unitOfWork)
        {
        }

        public async Task<ListViewModel<Anomaly>> FilterAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 20)
        {
            var listViewModel = new ListViewModel<Anomaly>();
            listViewModel.CurrentPage = page;
            listViewModel.PageSize = pageSize;
            var skip = (page - 1) * pageSize;
            var totalItemsQueryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tCOUNT(anomalyId) AS TotalItems")
                .AppendLine("FROM")
            .AppendLine("\tanomaly");

            listViewModel.TotalRecords = await this.QuerySingleRawAsync<int>(totalItemsQueryBuilder.ToString(), null, cancellationToken);
            while (listViewModel.CurrentPage > 1 && skip >= listViewModel.TotalRecords)
            {
                listViewModel.CurrentPage--;
                skip = (listViewModel.CurrentPage - 1) * pageSize;
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
            .AppendLine("\tanomaly");

            queryBuilder.AppendLine("ORDER BY creationdate DESC");

            queryBuilder.AppendLine("LIMIT @pageSize OFFSET @skip");

            var parameters = new
            {
                skip,
                pageSize
            };

            listViewModel.Data = await this.QueryRawAsync(queryBuilder.ToString(), parameters, cancellationToken);
            return listViewModel;
        }
    }
}
