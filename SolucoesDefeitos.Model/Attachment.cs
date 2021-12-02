using SolucoesDefeitos.Model.Contracts;
using SolucoesDefeitos.Model.Enum;
using System;

namespace SolucoesDefeitos.Model
{
    public class Attachment : ICreationDate,
        IUpdateDate
    {
        public int AttachmentId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Description { get; set; }

        public AttachmentCategory Category { get; set; }

        public int AnomalyId { get; set; }

        public Anomaly Anomaly { get; set; }

        public string Storage { get; set; }
    }
}
