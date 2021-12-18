using SolucoesDefeitos.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace SolucoesDefeitos.Model
{
    public class Product : ICreationDate,
        IEnabled,
        IUpdateDate
    {
        [Key]
        public int ProductId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public bool Enabled { get; set; }
        
        public DateTime? UpdateDate { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public int? ProductGroupId { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
