using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public interface IEntityDml
    {
        Type Type { get; }

        string Insert { get; }

        string Update { get; }

        string Delete { get; }

        string Select { get; }

        string SelectByKey { get; }
    }
}
