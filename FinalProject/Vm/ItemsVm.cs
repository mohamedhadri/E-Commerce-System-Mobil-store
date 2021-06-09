using FinalProject.Enum;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Vm
{
    public class ItemsVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public Color Color { get; set; }

        public int CategoryId { get; set; }
     
        public int BrandId { get; set; }
        public double Price { get; set; }
        public string ImageId { get; set; }

        public string BrandName { get; set; }

        public string CategoryName { get; set; }
        public List<string> Images { get; set; }

        public Items ToEntity()
        {
            return new Items
            {
                Name = Name,
                Description = Description,
                ShortDescription = ShortDescription,
                Price = Price,
                BrandId = BrandId,
                CategoryId = CategoryId,
                Color = Color
            };
        }

    }
}
