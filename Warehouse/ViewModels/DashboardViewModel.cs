namespace Warehouse.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalExpenses { get; set; }
        public Dictionary<string, int> OrdersByMonth { get; set; } // 儲存訂單數量
    }

}
