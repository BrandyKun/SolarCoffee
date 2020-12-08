using System;

namespace SolarCoffee.Data.Models
{
    public class ProductInvetory
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int QuantityHold { get; set; }
        public int IdealQuantity { get; set; }
        public Product Product { get; set; }
    }
}