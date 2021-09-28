using SolucoesDefeitos.Model.Contracts;
using System;
using System.Collections.Generic;

namespace SolucoesDefeitos.Model
{
    public class Anomaly : ICreationDate,
        IUpdateDate
    {
        public int AnomalyId { get; set; }

        public DateTime CreationDate { get ; set ; }

        public DateTime? UpdateDate { get ; set ; }

        public string Summary { get; set; }

        public string Description { get; set; }        

        public string RepairSteps { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
