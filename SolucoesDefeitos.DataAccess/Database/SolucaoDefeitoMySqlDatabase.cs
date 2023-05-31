using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SolucoesDefeitos.BusinessDefinition;
using System.Data;
using System.Data.Common;

namespace SolucoesDefeitos.DataAccess.Database
{
    public class SolucaoDefeitoMySqlDatabase : IDatabase
    {
        private readonly IConfiguration _configuration;
        private static DbConnection _dbConnection;

        public SolucaoDefeitoMySqlDatabase(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public DbConnection DbConnection
        {
            get
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                {
                    return _dbConnection;
                }

                _dbConnection = new MySqlConnection(_configuration.GetConnectionString("database"));
                return _dbConnection;
            }
        }

        public DbTransaction DbTransaction { get; set; }        
    }
}
