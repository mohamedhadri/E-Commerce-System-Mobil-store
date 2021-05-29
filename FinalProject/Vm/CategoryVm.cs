using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Vm
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageId { get; set; }

        public Categories ToEntity()
        {
            return new Categories()
            {
                Name = Name,
                Id = Id,
                Description = Description,
                ShortDescription = ShortDescription,

            };
           
        }

    }
}
