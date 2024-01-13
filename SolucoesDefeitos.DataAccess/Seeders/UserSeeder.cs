using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.Model;
using System;
using System.Text;

namespace SolucoesDefeitos.DataAccess.Seeders
{
    public sealed class UserSeeder : ISeeder
    {
        public void Seed(IDatabase database)
        {
            var sqlBuilder = new StringBuilder()
                .Append("SELECT")
                .AppendLine("\t(SELECT COUNT(userid) FROM user) > 0 AS recordExists");

            var recordExists = database.DbConnection.QuerySingleOrDefault<bool>(sqlBuilder.ToString());
            if (!recordExists)
            {
                var insertSqlBuilder = new StringBuilder()
                    .Append("INSERT INTO")
                    .AppendLine("\tuser ")
                    .Append("(creationdate, ")
                    .Append("enabled, ")
                    .Append("name, ")
                    .Append("login, ")
                    .Append("email, ")
                    .Append("password)")
                    .AppendLine("VALUES ")
                    .Append("(@creationdate, ")
                    .Append("@enabled, ")
                    .Append("@name, ")
                    .Append("@login, ")
                    .Append("@email, ")
                    .Append("@password);");
                var user = new User
                {
                    CreationDate = DateTime.Now,
                    Email = "admin@email.com",
                    Enabled = true,
                    Login = "admin",
                    Name = "admin",
                    Password = GetMd5Hash("12345")
                };

                database.DbConnection.Execute(insertSqlBuilder.ToString(), user);
            }
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
    }
}
