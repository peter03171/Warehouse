using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Warehouse.Data;
using Warehouse.Models;
using Warehouse.ViewModels;


namespace Warehouse.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly WarehouseContext _context;
        public OrdersController (WarehouseContext context)
        { 
            _context = context; 
        }

        // Orders Index 展示所有當前和歷史訂單

        public IActionResult Index(String searchString)
        {
            //tolist會直接從DB中撈出資料,後面如果有用到查詢則不該如此操作
            //var orders = (from m in _context.Orders select m).ToList();
            var orders = (from m in _context.Orders select m);

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(m => m.CustomerID.Equals(searchString));
            }
            return View(orders);
        }

        // 創建新的訂單
        //public IActionResult Create([Bind("OrderID,CustomerID,OrderDate,ShippedDate,Status,UserID")] Order Order)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        _context.Add(Order);
        //        _context.SaveChanges();
        //        return View(Order);

        //    }

        //    return View(Order);
        //}
        #region CreateOrder_&_OrderDeatai_Test
       
        //}

        [HttpPost]
        public async Task<IActionResult> CreateOrder2([FromBody] Order model)
        {
            //if (!ModelState.IsValid)
            //{
            //    Console.WriteLine("error");
            //    return View(model);
            //}
            try
            {
                if (model.OrderDate==DateTime.MinValue)
                {
                    model.OrderDate = DateTime.Today;
                }
                if (model.ShippedDate == DateTime.MinValue)
                {
                    model.ShippedDate = DateTime.Today;
                }
                var order = new Order
                {
                    CustomerID = model.CustomerID,
                    OrderDate = model.OrderDate,
                    ShippedDate = model.ShippedDate,
                    Status = model.Status
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return View(model);
        }


        // Postman 測試新增OrderDetail
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detail = new OrderDetail
            {
                OrderID = model.OrderID,
                ProductID = model.ProductID,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
            };

            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync(); // async

            //返回index
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail2([FromBody] List<OrderDetail> orderDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OrderDetails.AddRange(orderDetail);
            await _context.SaveChangesAsync();

            return Ok(orderDetail);
        }
        #endregion
        //[HttpGet]
        //public IActionResult CreateOrderWithDetails()
        //{
        //    var model = new OrderWithDetailsViewModel();
        //    return View(model);
        //}
        #region CreateOrderWithDetails 
        public async Task<IActionResult> CreateOrderWithDetails()
        {
            var model = new OrderWithDetailsViewModel()
            {
                Products = await _context.Products
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice
                    }).ToListAsync()
            };

            return View(model);
        }
        
        //[Consumes("application/json")]
        //[Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CreateOrderWithDetails(OrderWithDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // 檢查和設置預設日期
                //if (model.Order.OrderDate == DateTime.MinValue)
                //{
                //    model.Order.OrderDate = DateTime.Today;
                //}
                //if (model.Order.ShippedDate == DateTime.MinValue)
                //{
                //    model.Order.ShippedDate = DateTime.Today;
                //}

                // 添加 Order
                _context.Orders.Add(model.Order);
                await _context.SaveChangesAsync();

                // 為每個 OrderDetail 設置當前 Order 的 ID
                foreach (var detail in model.OrderDetails)
                {
                    detail.OrderID = model.Order.OrderID; // 確保 OrderDetail 關聯到剛剛創建的 Order
                    _context.OrderDetails.Add(detail);
                }
                await _context.SaveChangesAsync();

                //return Ok();
                return RedirectToAction(nameof(Index));// 表單送出導回index
            }
            catch (Exception ex)
            {
                // 處理異常情況
                Console.WriteLine($"An error occurred: {ex.Message}");
                //return BadRequest();// badrequest 為Client 請求異常(400) 若為Server內部(try catch) 應為500 適當
                return StatusCode(500,"Internal server error");
            }
        }
        //[HttpGet]
        //[Route("api/productlist")]
        //public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        //{

        //    var products = await _context.Products
        //        .Select(p => new ProductViewModel
        //        {
        //            ProductId = p.ProductId,
        //            ProductName = p.ProductName,
        //            UnitPrice = p.UnitPrice
        //        }).ToListAsync();

        //    return products;
        //}
        #endregion
        //---------TODO: Edit導向 orderdetail----------
        #region Edit


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderEditViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                CustomerID = order.CustomerID,
                Status = order.Status,
                Customers = new SelectList(await _context.Customers.ToListAsync(), "CustomerID", "CompanyName") // 
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderEditViewModel viewModel)
        {
            //if (id != viewModel.OrderID)
            //{
            //    return NotFound();
            //}
            Console.WriteLine($"OrderID: {viewModel.OrderID}");
            Console.WriteLine($"OrderDate: {viewModel.OrderDate}");
            Console.WriteLine($"ShippedDate: {viewModel.ShippedDate}");
            Console.WriteLine($"CustomerID: {viewModel.CustomerID}");
            Console.WriteLine($"Status: {viewModel.Status}");
            ModelState.Remove("Customers");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (ModelState.IsValid)
            {
                var orderToUpdate = await _context.Orders.FindAsync(viewModel.OrderID);
                if (orderToUpdate == null)
                {
                    return NotFound();
                }

                orderToUpdate.OrderDate = viewModel.OrderDate;
                orderToUpdate.ShippedDate = viewModel.ShippedDate;
                orderToUpdate.CustomerID = viewModel.CustomerID;
                orderToUpdate.Status = viewModel.Status;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // 
            }

            // 重新加载Customers SelectList
            //viewModel.Customers = new SelectList(await _context.Customers.ToListAsync(), "CustomerID", "CompanyName", viewModel.CustomerID);
            return RedirectToAction(nameof(Index));
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        #endregion

        #region OrderDetail


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderID == id.Value)
                .ToListAsync();

            var productIds = orderDetails.Select(od => od.ProductID).Distinct();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            var viewModel = new OrderDetailsViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                Status = order.Status,
                Products = orderDetails.Select(od => new ProductInfo
                {
                    ProductName = products.FirstOrDefault(p => p.ProductId == od.ProductID)?.ProductName,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            return View(viewModel);
        }







        #endregion



        #region OrderDelete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // OrderDetails
            var orderDetails = await _context.OrderDetails
                                     .Where(od => od.OrderID == id)
                                     .ToListAsync();

            foreach (var detail in orderDetails)
            {
                _context.OrderDetails.Remove(detail);
            }

            // 
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

    }




}
