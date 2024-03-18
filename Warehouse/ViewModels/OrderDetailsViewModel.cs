namespace Warehouse.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string Status { get; set; }
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();
    }

    public class ProductInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}


