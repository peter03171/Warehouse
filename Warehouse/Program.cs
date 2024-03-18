
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WarehouseContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseContext") ?? throw new InvalidOperationException("Connection string 'WarehouseContext' not found.")));

// Add services to the container.
// services.AddControllersWithViews()
//包含 AddControllers() 的所有項目，再加上：

// 1.cshtml Razor View
// 2.Cache Tag Helper

//builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("Cookieauth") // Cookie方案名稱
    .AddCookie("Cookieauth", options =>
    {
        options.LoginPath = "/User/Login"; // 登入路徑
        // 配置其他選項
    });
//    builder.Services.AddDbContext<WarehouseDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();// 身分驗證
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
