using SolucoesDefeitos.Model.Contracts;
using System;

namespace SolucoesDefeitos.Model
{
    public class Product : ICreationDate,
        IEnabled,
        IUpdateDate
    {
        public int ProductId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public bool Enabled { get; set; }
        
        public DateTime? UpdateDate { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public int? ProductGroupId { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
