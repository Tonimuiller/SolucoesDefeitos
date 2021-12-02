using SolucoesDefeitos.DataAccess.EntityDml;
using System.Collections.Generic;
using System.Data;

namespace SolucoesDefeitos.DataAccess.Database
{
    public interface IDatabase
    {
        IDbConnection DbConnection { get; }

        IEnumerable<IEntityDml> EntitiesDmls { get; }
    }
}
