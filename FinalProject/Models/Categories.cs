using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Categories:BaseEntity<int>
    {
        public string Name { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [ForeignKey ("ParentId")]
        public int? ParentId { get; set; }
        public Categories Parent { get; set; }

    }
}
