using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [Table("Orders")]
    public class Order
    {
        //public Order()
        //{
        //    OrderDetails = new HashSet<OrderDetail>();
        //}
        [Key]
        public int OrderID { get; set; }
        [Display(Name ="公司名稱")]
        public int CustomerID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShippedDate { get; set; }
        
        public String Status { get; set; }
        public int? UserID { get; set; }

        //public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        //public List<Product> Products { get; set; } = new List<Product>();

    }
}
