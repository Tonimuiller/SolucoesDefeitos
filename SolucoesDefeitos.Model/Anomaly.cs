using SolucoesDefeitos.Model.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolucoesDefeitos.Model
{
    public class Anomaly : ICreationDate,
        IUpdateDate
    {
        [Key]
        public int AnomalyId { get; set; }

        public DateTime CreationDate { get ; set ; }

        public DateTime? UpdateDate { get ; set ; }

        [Required]
        public string Summary { get; set; }

        public string Description { get; set; }        

        public string RepairSteps { get; set; }

        public ICollection<AnomalyProductSpecification> ProductSpecifications { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
