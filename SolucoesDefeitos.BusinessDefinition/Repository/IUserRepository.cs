using SolucoesDefeitos.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessDefinition.Repository
{
    public interface IUserRepository: IRepository<User, int>
    {
        Task<User> GetByCredentialsAsync(string loginOrEmail, string password, CancellationToken cancellationToken);

        Task<bool> IsLoginAvailableAsync(string login, int? userId, CancellationToken cancellationToken);

        Task<bool> IsEmailAvailableAsync(string email, int? userId, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int userId, CancellationToken cancellationToken);
    }
}
