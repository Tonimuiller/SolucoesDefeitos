using SolucoesDefeitos.Model.Contracts;
using System;
using System.Collections.Generic;

namespace SolucoesDefeitos.Model
{
    public class ProductGroup : ICreationDate,
        IEnabled,
        IUpdateDate
    {
        public int ProductGroupId { get; set; }

        public DateTime CreationDate { get ; set ; }

        public bool Enabled { get ; set ; }

        public DateTime? UpdateDate { get ; set ; }

        public string Description { get; set; }

        public int? FatherProductGroupId { get; set; }

        public virtual ProductGroup FatherProductGroup { get; set; }

        public virtual ICollection<ProductGroup> Subgroups { get; set; }
    }
}
