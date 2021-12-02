using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class ProductGroupEntityDml : IEntityDml
    {        
        public string Insert => 
            @"INSERT INTO `solucaodefeito`.`productgroup`
                (`creationdate`,
                `enabled`,
                `description`,
                `fatherproductgroupid`)
            VALUES
                (@CreationDate,
                @Enabled,
                @Description,
                @FatherProductGroupId)";
               
        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`productgroup`
            SET
                `updatedate` = @UpdateDate,
                `enabled` = @Enabled,
                `description` = @Description,
                `fatherproductgroupid` = @FatherProductGroupId
            WHERE 
                `productgroupid` = @ProductGroupId";
               
        public string Delete =>
            @"DELETE FROM
                `solucaodefeito`.`productgroup` 
            WHERE 
                `productgroupid` = @ProductGroupId";
               
        public string Select =>
            @"SELECT 
                `productgroup`.`productgroupid`,
                `productgroup`.`creationdate`,
                `productgroup`.`updatedate`,
                `productgroup`.`enabled`,
                `productgroup`.`description`,
                `productgroup`.`fatherproductgroupid`           
            FROM `solucaodefeito`.`productgroup`";

        public Type Type => typeof(ProductGroup);

        public string SelectByKey => 
            $@"{this.Select} 
            WHERE 
                `productgroup`.`productgroupid` = @productgroupid";
        
        public string SelectByFatherProductGroupId =>
            $@"{this.Select}
            WHERE
                `productgroup`.`fatherproductgroupid` = @productgroupid";
    }
}
