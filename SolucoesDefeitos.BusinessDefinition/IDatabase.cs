using System.Data.Common;

namespace SolucoesDefeitos.BusinessDefinition
{
    public interface IDatabase
    {
        public DbConnection DbConnection { get; }

        public DbTransaction DbTransaction { get; set; }
    }
}
