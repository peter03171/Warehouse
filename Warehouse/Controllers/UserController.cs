using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Warehouse.Data;

namespace Warehouse.Controllers
{
    public class UserController : Controller
    {
        private WarehouseContext _context;
        public UserController(WarehouseContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            ModelState.Remove("Role");
            if (ModelState.IsValid)
            {
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.PassWord == model.PassWord);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("UserId", user.UserId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync("Cookieauth", new ClaimsPrincipal(claimsIdentity));


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "無效的登入嘗試。");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Profile()
        {
            var userName = User.Identity.Name; // ClaimsIdentity獲取

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user != null)
            {
                return View(user); // 模型傳回view
            }
            else
            {
                return RedirectToAction("Login"); // 無用戶則導回Login
            }
        }

    }
}
