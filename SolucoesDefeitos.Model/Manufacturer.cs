using SolucoesDefeitos.Model.Contracts;
using System;
using System.Collections.Generic;

namespace SolucoesDefeitos.Model
{
    public class Manufacturer: ICreationDate,
        IEnabled,
        IUpdateDate
    {
        public int ManufacturerId { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public bool Enabled { get; set; }
        
        public DateTime? UpdateDate { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
