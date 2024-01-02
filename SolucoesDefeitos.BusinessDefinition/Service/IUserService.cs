using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Service
{
    public interface IUserService
    {
        Task<User> GetByCredentialsAsync(string userNameEmail, string password, CancellationToken cancellationToken);
    }
}
