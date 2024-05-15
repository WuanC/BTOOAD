using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OOAD.Models;

namespace OOAD.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.CalenderID == null)
                {
                    // Loại bỏ CalenderID từ ModelState để ngăn MVC Framework liên kết dữ liệu từ form
                    ModelState.Remove("CalenderID");
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


   
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model, string returnUrl = null)
        {
            var _users = await _context.Users.ToListAsync();
            if (ModelState.IsValid)
            {
                var user = _users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    string encodedUserID = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.UserID.ToString()));

                    Response.Cookies.Append("UserID", encodedUserID);

                    // Chuyển hướng người dùng đến trang returnUrl nếu có, hoặc trang chính
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {

                        return RedirectToAction("Index", "HomeController");
                    }
                    else
                    {
                        Console.WriteLine("Check");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu");
                }

            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserID");
            return RedirectToAction("Login", "Users");
        }






    }
}
