using MySql.Data.MySqlClient;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Provider;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.UnitOfWork
{
    public class SolucaoDefeitoUnitOfWork : DapperUnitOfWork<SolucaoDefeitoMySqlDatabase>,
        IUnitOfWork
    {
        public SolucaoDefeitoUnitOfWork(
            IDateTimeProvider dateTimeProvider,
            SolucaoDefeitoMySqlDatabase database
            ) : base(dateTimeProvider, database)
        {
        }

        public override async Task<ResponseDto> DeleteAsync<T>(T entity)
        {
            try
            {
                return await base.DeleteAsync(entity);
            }
            catch (MySqlException mysqlEx)
            {
                if (mysqlEx.Number == this.Database.ForeignKeyRelationshipViolationErrorCode)
                {
                    return new ResponseDto(false, "Can't delete record due to foreign key relationship violation.");
                }
            }
            catch
            {
                throw;
            }

            return new ResponseDto(false, "Delete not executed.");
        }
    }
}
