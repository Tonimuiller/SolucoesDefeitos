using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.UnitOfWork;
using SolucoesDefeitos.Model;

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
    }
}
