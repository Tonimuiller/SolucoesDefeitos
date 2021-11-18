using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class AnomalyEntityDml : IEntityDml
    {
        public Type Type => typeof(Anomaly);

        public string Insert =>
            @"INSERT INTO `solucaodefeito`.`anomaly`
                (`creationdate`,
                `summary`,
                `description`,
                `repairsteps`)
            VALUES
                (@creationdate,
                @summary,
                @description,
                @repairsteps)";

        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`anomaly`
            SET
                `updatedate` = @updatedate,
                `summary` = @summary,
                `description` = @description,
                `repairsteps` = @repairsteps
            WHERE 
                `anomalyid` = @anomalyid";

        public string Delete =>
            @"DELETE FROM 
                `solucaodefeito`.`anomaly`
            WHERE 
                `anomaly`.`anomalyid` = @anomalyid";

        public string Select =>
            @"SELECT 
                `anomaly`.`anomalyid`,
                `anomaly`.`creationdate`,
                `anomaly`.`updatedate`,
                `anomaly`.`summary`,
                `anomaly`.`description`,
                `anomaly`.`repairsteps`
            FROM 
                `solucaodefeito`.`anomaly`";

        public string SelectByKey => 
            $@"{this.Select}
            WHERE
                `anomaly`.`anomalyid` = @anomalyid";
    }
}
