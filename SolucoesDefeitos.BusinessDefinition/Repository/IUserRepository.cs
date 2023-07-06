using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IUserRepository: IRepository<User, int>
    {
        Task<ResponseDto<User>> GetByCredentialsAsync(string loginOrEmail, string password, CancellationToken cancellationToken);
    }
}
