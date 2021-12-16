using MySql.Data.MySqlClient;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.DataAccess.Database;
using SolucoesDefeitos.DataAccess.Exception;
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
                this.HandleIfForeignKeyRelationshipViolationException(mysqlEx.Number);
            }
            catch
            {
                throw;
            }

            return new ResponseDto(false, "Delete not executed.");
        }

        private void HandleIfForeignKeyRelationshipViolationException(int errorNumber)
        {
            if (errorNumber == this.Database.ForeignKeyRelationshipViolationErrorCode)
            {
                throw new RecordDependencyBreakException();
            }
        }
    }
}
