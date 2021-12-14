using SolucoesDefeitos.DataAccess.EntityDml;
using SolucoesDefeitos.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SolucoesDefeitos.DataAccess.Database
{
    public interface IDatabase
    {
        IEnumerable<IEntityDml> EntitiesDmls { get; }

        DataTable GetSchema(string collectionName);

        Task<ResponseDto<int>> ExecuteAsync(string command, object entity);

        // Task<ResponseDto<int>> ExecuteEscalarAsync(s)
    }
}
