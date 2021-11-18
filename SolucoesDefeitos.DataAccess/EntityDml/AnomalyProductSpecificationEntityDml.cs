using SolucoesDefeitos.Model;
using System;

namespace SolucoesDefeitos.DataAccess.EntityDml
{
    public class AnomalyProductSpecificationEntityDml : IEntityDml
    {
        public Type Type => typeof(AnomalyProductSpecification);

        public string Insert =>
            @"INSERT INTO `solucaodefeito`.`anomalyproductspecification`
                (`creationdate`,
                `anomalyid`,
                `productid`,
                `manufactureyear`)
            VALUES
                (@creationdate,
                @anomalyid,
                @productid,
                @manufactureyear)";

        public string Update =>
            @"UPDATE 
                `solucaodefeito`.`anomalyproductspecification`
            SET
                `updatedate` = @updatedate,
                `anomalyid` = @anomalyid,
                `productid` = @productid,
                `manufactureyear` = @manufactureyear
            WHERE 
                `anomalyproductspecificationid` = @anomalyproductspecificationid";

        public string Delete =>
            @"DELETE FROM 
                `solucaodefeito`.`anomalyproductspecification`
            WHERE 
                `anomalyproductspecification`.`anomalyproductspecificationid` = @anomalyproductspecificationid";

        public string Select =>
            @"SELECT 
                `anomalyproductspecification`.`anomalyproductspecificationid`,
                `anomalyproductspecification`.`creationdate`,
                `anomalyproductspecification`.`updatedate`,
                `anomalyproductspecification`.`anomalyid`,
                `anomalyproductspecification`.`productid`,
                `anomalyproductspecification`.`manufactureyear`
            FROM 
                `solucaodefeito`.`anomalyproductspecification`";

        public string SelectByKey => 
            $@"{this.Select}
            WHERE
                `anomalyproductspecification`.`anomalyproductspecificationid` = @anomalyproductspecificationid";
    }
}
