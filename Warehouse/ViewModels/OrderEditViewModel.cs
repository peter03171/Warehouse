using Microsoft.AspNetCore.Mvc.Rendering;

namespace Warehouse.ViewModels
{
    public class OrderEditViewModel
    {
        public int OrderID { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }
        public int CustomerID { get; set; }
        public string Status { get; set; }
        public SelectList Customers { get; set; } // 客户下拉列表
    }

}
