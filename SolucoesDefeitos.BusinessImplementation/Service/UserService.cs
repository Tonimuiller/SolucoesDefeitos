using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.BusinessDefinition.Service;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolucoesDefeitos.BusinessImplementation.Service
{
    public sealed class UserService : IService<User, int>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseDto<User>> AddAsync(User entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var validationErrors = await ValidateUserAsync(entity, cancellationToken);
            if (validationErrors.Any())
            {
                return new ResponseDto<User>(validationErrors.ToArray());
            }

            var addedUser = await _userRepository.AddAsync(entity, cancellationToken);
            return new ResponseDto<User>(true, addedUser);
        }

        public async Task<ResponseDto> DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (! await _userRepository.ExistsAsync(entity.UserId, cancellationToken))
            {
                return new ResponseDto(false, "Não foi encontrado o usuário com o identificador informado.");
            }

            return await _userRepository.DeleteAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User> GetByCredentialsAsync(string userNameEmail, string password, CancellationToken cancellationToken)
        {
            var hashedPassword = GetMd5Hash(password);
            return await _userRepository.GetByCredentialsAsync(userNameEmail, hashedPassword, cancellationToken);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<ResponseDto> UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (! await _userRepository.ExistsAsync(entity.UserId, cancellationToken))
            {
                return new ResponseDto<User>("Não foi encontrado o usuário com o identificador informado.");
            }

            var validationErrors = await ValidateUserAsync(entity, cancellationToken);
            if (validationErrors.Any())
            {
                return new ResponseDto<User>(validationErrors.ToArray());
            }

            await _userRepository.UpdateAsync(entity, cancellationToken);
            return new ResponseDto(true);
        }

        private string GetMd5Hash(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        private async Task<IReadOnlyList<string>> ValidateUserAsync(User user, CancellationToken cancellationToken)
        {
            var validationErrors = new List<string>();
            if (string.IsNullOrEmpty(user.Name))
            {
                validationErrors.Add("O nome do usuário é obrigatório.");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                validationErrors.Add("O e-mail do usuário é obrigatório.");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                validationErrors.Add("A senha do usuário é obrigatória.");
            }

            int? userId = user.UserId > 0 ? user.UserId : default;
            if (! await _userRepository.IsLoginAvailableAsync(user.Login, userId, cancellationToken))
            {
                validationErrors.Add("O login informado não está disponível.");
            }

            if (! await _userRepository.IsEmailAvailableAsync(user.Email, userId, cancellationToken))
            {
                validationErrors.Add("O e-mail informado não está disponível.");
            }

            return validationErrors;
        }
    }
}
