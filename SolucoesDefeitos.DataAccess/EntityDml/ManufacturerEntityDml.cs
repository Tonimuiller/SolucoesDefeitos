using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class ManufacturerEntityDml : IEntityDml
    {
        public Type Type => typeof(Manufacturer);

        public string Insert =>
            @"INSERT INTO `solucaodefeito`.`manufacturer`
                (`creationdate`,
                `enabled`,
                `name`)
            VALUES
                (@creationdate,
                @enabled,
                @name)";

        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`manufacturer`
            SET
                `updatedate` = @updatedate,
                `enabled` = @enabled,
                `name` = @name
            WHERE 
                `manufacturerid` = @manufacturerid;";

        public string Delete =>
            @"DELETE FROM 
                `solucaodefeito`.`manufacturer`
            WHERE 
                `manufacturerid` = @manufacturerid;";

        public string Select =>
            @"SELECT 
                `manufacturer`.`manufacturerid`,
                `manufacturer`.`creationdate`,
                `manufacturer`.`updatedate`,
                `manufacturer`.`enabled`,
                `manufacturer`.`name`
            FROM `solucaodefeito`.`manufacturer`";

        public string SelectByKey => 
            $@"{this.Select}
            WHERE
                `manufacturer`.`manufacturerid` = @manufacturerid";
    }
}
