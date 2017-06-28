using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }        
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductExt : Product
    {
        public string Description { get; set; }        
    }
}
