using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;

namespace Warehouse.Data
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext (DbContextOptions<WarehouseContext> options)
            : base(options)
        {
        }

        public DbSet<Warehouse.Models.Product> Products { get; set; }
        public DbSet<Warehouse.Models.Order> Orders { get; set; }
        public DbSet<Warehouse.Models.OrderDetail> OrderDetails { get; set; }
        public DbSet<Warehouse.Models.Customer> Customers { get; set; }
        public DbSet<Warehouse.Models.Category> Categories { get; set; }
        public DbSet<Warehouse.Models.User> User { get; set; }
    }
}
