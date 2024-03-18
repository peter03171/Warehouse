using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [Table("products")]
    public class Product
    {

        //public Product()
        //{
        //    OrderDetails = new HashSet<OrderDetail>();
        //}
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityPerUnit { get; set; }

        //其他属性...
        //public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }

}
