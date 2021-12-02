using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SolucoesDefeitos.DataAccess.EntityDml;
using System.Collections.Generic;
using System.Data;

namespace SolucoesDefeitos.DataAccess.Database
{
    public class SolucaoDefeitoMySqlDatabase : IDatabase
    {
        private readonly IConfiguration configuration;
        private IDbConnection dbConnection;

        public SolucaoDefeitoMySqlDatabase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<IEntityDml> EntitiesDmls
        {
            get
            {
                yield return new ProductGroupEntityDml();
            }
        }

        public IDbConnection DbConnection
        {
            get
            {
                if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                {
                    return dbConnection;
                }

                dbConnection = new MySqlConnection(configuration.GetConnectionString("database"));
                return dbConnection;
            }
        }
    }
}
