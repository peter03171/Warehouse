using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        //[ForeignKey("OrderID")]
        //public virtual Order Order { get; set; }

        //[ForeignKey("ProductID")]
        //public virtual Product Product { get; set; }
    }
}
