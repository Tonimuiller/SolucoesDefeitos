using Dapper;
using SolucoesDefeitos.BusinessDefinition;
using SolucoesDefeitos.BusinessDefinition.Repository;
using SolucoesDefeitos.Dto;
using SolucoesDefeitos.Model;
using SolucoesDefeitos.Provider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SolucoesDefeitos.DataAccess.Repository
{
    public class AttachmentRepository: IAttachmentRepository
    {
        private readonly IDatabase _database;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AttachmentRepository(
            IDatabase database, 
            IDateTimeProvider dateTimeProvider)
        {
            _database = database;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Attachment> AddAsync(Attachment entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("INSERT INTO attachment")
                .AppendLine("\t(creationdate,")
                .AppendLine("\tdescription,")
                .AppendLine("\tcategory,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tstorage)")
                .AppendLine("VALUES")
                .AppendLine("\t(@creationdate,")
                .AppendLine("\t@description,")
                .AppendLine("\t@category,")
                .AppendLine("\t@anomalyid,")
                .AppendLine("\t@storage);")
                .AppendLine("SELECT LAST_INSERT_ID();");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            entity.AttachmentId = await _database.DbConnection.ExecuteScalarAsync<int>(commandDefinition);
            return entity;
        }

        public async Task<ResponseDto> DeleteAsync(Attachment entity, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM attachment WHERE attachmentid = @attachmentid",
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            try
            {
                await _database.DbConnection.ExecuteAsync(commandDefinition);
                return new ResponseDto(true);
            }
            catch (Exception ex)
            {
                return new ResponseDto(false, ex.Message);
            }
        }

        public async Task DeleteAsync(int anomalyId, int[] attachmentIds, CancellationToken cancellationToken)
        {
            var commandDefinition = new CommandDefinition(
                "DELETE FROM attachment WHERE anomalyid = @anomalyid and attachmentid NOT IN @attachmentids",
                new
                {
                    anomalyId,
                    attachmentIds
                },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync(CancellationToken cancellationToken)
        {
            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tattachmentid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tdescription,")
                .AppendLine("\tcategory,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tstorage")
                .AppendLine("FROM")
                .AppendLine("\tattachment");
            var commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                transaction: _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<Attachment>(commandDefinition);
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsByAnomalyIdAsync(int anomalyId, CancellationToken cancellationToken)
        {
            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tattachmentid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tdescription,")
                .AppendLine("\tcategory,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tstorage")
                .AppendLine("FROM")
                .AppendLine("\tattachment")
                .AppendLine("WHERE")
                .AppendLine("\tanomalyid = @anomalyid");
            var commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                new { anomalyId },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QueryAsync<Attachment>(commandDefinition);
        }

        public async Task<Attachment> GetByIdAsync(int keyValue, CancellationToken cancellationToken)
        {
            var queryBuilder = new StringBuilder()
                .AppendLine("SELECT")
                .AppendLine("\tattachmentid,")
                .AppendLine("\tcreationdate,")
                .AppendLine("\tupdatedate,")
                .AppendLine("\tdescription,")
                .AppendLine("\tcategory,")
                .AppendLine("\tanomalyid,")
                .AppendLine("\tstorage")
                .AppendLine("FROM")
                .AppendLine("\tattachment")
                .AppendLine("WHERE")
                .AppendLine("\tattachmentid = @tattachmentid");
            var commandDefinition = new CommandDefinition(
                queryBuilder.ToString(),
                new { attachmentId = keyValue },
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            return await _database.DbConnection.QuerySingleOrDefaultAsync<Attachment>(commandDefinition);
        }

        public async Task UpdateAsync(Attachment entity, CancellationToken cancellationToken)
        {
            entity.UpdateDate = _dateTimeProvider.CurrentDateTime;
            var sqlBuilder = new StringBuilder()
                .AppendLine("UPDATE")
                .AppendLine("\tattachment")
                .AppendLine("SET")
                .AppendLine("\tupdatedate = @updatedate,")
                .AppendLine("\tdescription = @description,")
                .AppendLine("\tcategory = @category,")
                .AppendLine("\tanomalyid = @anomalyid,")
                .AppendLine("\tstorage = @storage")
                .AppendLine("WHERE")
                .AppendLine("\tattachmentid = @attachmentid");
            var commandDefinition = new CommandDefinition(
                sqlBuilder.ToString(),
                entity,
                _database.DbTransaction,
                cancellationToken: cancellationToken);
            await _database.DbConnection.ExecuteAsync(commandDefinition);
        }
    }
}
