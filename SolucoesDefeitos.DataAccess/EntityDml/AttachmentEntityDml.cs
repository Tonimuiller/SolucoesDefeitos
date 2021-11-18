using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class AttachmentEntityDml : IEntityDml
    {
        public Type Type => typeof(Attachment);

        public string Insert =>
            @"INSERT INTO `solucaodefeito`.`attachment`
                (`creationdate`,
                `description`,
                `category`,
                `anomalyid`,
                `storage`)
            VALUES
                (@creationdate,
                @description,
                @category,
                @anomalyid,
                @storage)";

        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`attachment`
            SET
                `updatedate` = @updatedate,
                `description` = @description,
                `category` = @category,
                `anomalyid` = @anomalyid,
                `storage` = @storage
            WHERE 
                `attachmentid` = @attachmentid";

        public string Delete =>
            @"DELETE FROM 
                `solucaodefeito`.`attachment`
            WHERE 
                `attachment`.`attachmentid` = @attachmentid";

        public string Select =>
            @"SELECT 
                `attachment`.`attachmentid`,
                `attachment`.`creationdate`,
                `attachment`.`updatedate`,
                `attachment`.`description`,
                `attachment`.`category`,
                `attachment`.`anomalyid`,
                `attachment`.`storage`
            FROM 
                `solucaodefeito`.`attachment`";

        public string SelectByKey => 
            $@"{this.Select}
            WHERE
                `attachment`.`attachmentid` = @attachmentid";
    }
}
