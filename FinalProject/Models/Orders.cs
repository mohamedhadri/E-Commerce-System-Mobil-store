using FinalProject.Vm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Orders: BaseEntity<int>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public int ItemId { get; set; }
        [ForeignKey ("ItemId")]

        public Items Items { get; set; }

        public OrderStatus OrderStatus { get; set; }


    }
}
