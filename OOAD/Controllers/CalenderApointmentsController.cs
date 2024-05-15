using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OOAD.Models;

namespace OOAD.Controllers
{
    public class CalenderApointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public CalenderApointmentsController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            string encodedUserID = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(encodedUserID))
            {
                return NotFound("Bạn chưa đăng nhập");
            }
            var appDbContext = _context.Calenders;
            return View(await appDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

        var calenderApointment = await _context.Calenders
        .FirstOrDefaultAsync(m => m.CalenderID == id);
            if (calenderApointment == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Password");
            return View(calenderApointment);
        }

        public IActionResult Create()
        {
            string encodedUserID = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(encodedUserID))
            {
                return NotFound("Bạn chưa đăng nhập");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CalenderID,Name,Location,Start,End")] CalenderApointment calenderApointment)
        {
            if (ModelState.IsValid)
            {
                string encodedUserID = HttpContext.Request.Cookies["UserID"];
                if (string.IsNullOrEmpty(encodedUserID))
                {

                    return NotFound("Bạn chưa đăng nhập");
                }

                int userID;
                if (!int.TryParse(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserID)), out userID))
                {

                    return NotFound("Invalid UserID.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
                if (user == null)
                {

                    return NotFound("Không tìm thấy user.");
                }
                bool isOverlapping = await IsAppointmentOverlappingAsync(calenderApointment, userID);

                if (!isOverlapping)
                {
                    calenderApointment.UserID = userID;
                    _context.Add(calenderApointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "lịch mới bị trùng lịch cũ");
                }
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Password", calenderApointment.UserID);
            return View(calenderApointment);
        }

        private async Task<bool> IsAppointmentOverlappingAsync(CalenderApointment newAppointment, int userID)
        {
           
            var existingAppointments = await _context.Calenders
                .Where(a => a.Start < newAppointment.End && a.End > newAppointment.Start && a.UserID == userID)
                .ToListAsync();
            return existingAppointments.Any();
        }



        public async Task<IActionResult> Edit(int? id)
        {
            string encodedUserID = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(encodedUserID))
            {

                return NotFound("Bạn chưa đăng nhập");
            }

            int userID;
            if (!int.TryParse(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserID)), out userID))
            {

                return NotFound("Invalid UserID.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            if (user == null)
            {

                return NotFound("Không tìm thấy user.");
            }

            var calenderApointment = await _context.Calenders.FindAsync(id);
            if (calenderApointment == null)
            {
                return NotFound();
            }
            if(calenderApointment.UserID != user.UserID)
            {
                return NotFound("Cuộc họp này không do bạn sở hữu");
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Password", calenderApointment.UserID);
            return View(calenderApointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CalenderID,Name,Location,Start,End,UserID")] CalenderApointment calenderApointment)
        {
            if (id != calenderApointment.CalenderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calenderApointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalenderApointmentExists(calenderApointment.CalenderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Password", calenderApointment.UserID);
            return View(calenderApointment);
        }


        private bool CalenderApointmentExists(int id)
        {
            return _context.Calenders.Any(e => e.CalenderID == id);
        }
        [HttpGet]
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calenderApointment = await _context.Calenders.FindAsync(id);
            if (calenderApointment == null)
            {
                return NotFound();
            }
          
            return View(calenderApointment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            string encodedUserID = HttpContext.Request.Cookies["UserID"];
            if (string.IsNullOrEmpty(encodedUserID))
            {

                return NotFound("Bạn chưa đăng nhập");
            }

            int userID;
            if (!int.TryParse(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUserID)), out userID))
            {
                return NotFound("Invalid UserID.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);
            if (user == null)
            {

                return NotFound("User not found in database.");
            }

            user.CalenderID = id;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
