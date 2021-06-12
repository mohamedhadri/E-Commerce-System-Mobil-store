using FinalProject.Enum;
using FinalProject.Vm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Items:BaseEntity<int>
    {
        public string Name { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public Color Color  { get; set; }

        
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Categories { get; set; }

       
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brands Brands { get; set; }
        public double Price { get; set; }

    
    }
}
