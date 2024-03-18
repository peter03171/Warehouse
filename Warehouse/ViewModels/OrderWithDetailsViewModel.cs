namespace Warehouse.ViewModels
{
    public class OrderWithDetailsViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Product>? Products { get; set; }
    }

}
