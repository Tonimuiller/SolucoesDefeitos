using SolucoesDefeitos.Model.Contracts;
using System;

namespace SolucoesDefeitos.Model
{
    public class AnomalyProductSpecification: ICreationDate,
        IUpdateDate
    {
        public int AnomalyProductSpecificationId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime? UpdateDate { get; set; }

        public int AnomalyId { get; set; }

        public virtual Anomaly Anomaly { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int ManufactureYear { get; set; }
    }
}
