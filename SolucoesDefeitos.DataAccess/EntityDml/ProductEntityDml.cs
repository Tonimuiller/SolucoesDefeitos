using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class ProductEntityDml : IEntityDml
    {
        public Type Type => typeof(Product);

        public string Insert =>
            @"INSERT INTO `solucaodefeito`.`product`
                (`creationdate`,
                `enabled`,
                `manufacturerid`,
                `productgroupid`,
                `name`,
                `code`)
            VALUES
                (@creationdate,
                @enabled,
                @manufacturerid,
                @productgroupid,
                @name,
                @code)";

        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`product`
            SET
                `updatedate` = @updatedate,
                `enabled` = @enabled,
                `manufacturerid` = @manufacturerid,
                `productgroupid` = @productgroupid,
                `name` = @name,
                `code` = @code
            WHERE 
                `productid` = @productid";

        public string Delete =>
            @"DELETE FROM 
                `solucaodefeito`.`product`
            WHERE 
                `productid` = @productid";

        public string Select =>
            @"SELECT 
                `product`.`productid`,
                `product`.`creationdate`,
                `product`.`updatedate`,
                `product`.`enabled`,
                `product`.`manufacturerid`,
                `product`.`productgroupid`,
                `product`.`name`,
                `product`.`code`
            FROM 
                `solucaodefeito`.`product`";

        public string SelectByKey => 
            $@"{this.Select} 
            WHERE
                `product`.`productid` = @productid";
    }
}
