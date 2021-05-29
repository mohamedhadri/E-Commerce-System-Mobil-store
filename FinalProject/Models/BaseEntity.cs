using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreateDate = DateTime.Now;
            IsDelete = false;
        }

        [Key]
        public T Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
