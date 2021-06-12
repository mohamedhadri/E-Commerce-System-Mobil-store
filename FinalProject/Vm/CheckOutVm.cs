using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Vm
{
    public class CheckOutVm
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Prepparing,
        OnTheWay,
        AtAddress
    }

}
