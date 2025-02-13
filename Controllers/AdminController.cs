using FribergCarRentalApp.Data;
using FribergCarRentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FribergCarRentalApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RentalAppDbContext rentalAppDbContext;

        public AdminController(RentalAppDbContext rentalAppDbContext)
        {
            this.rentalAppDbContext = rentalAppDbContext;
        }

        // Get: /admin/
        public IActionResult Index(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password, string returnUrl)
        {
            var ReturnUrl = returnUrl;
            var admin = rentalAppDbContext.Admins.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (admin == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View("Index");
            }

            HttpContext.Session.SetString("AdminName", admin.Name);
            HttpContext.Session.SetInt32("AdminId", admin.Id);
            

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            else
            {

                return RedirectToAction("Dashboard");
            }


        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("AdminName");
            HttpContext.Session.Remove("AdminId");

            return RedirectToAction("Index");
        }
        public IActionResult Dashboard()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ManageBookings()
        {
            var bookings = rentalAppDbContext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .ToList();

            return View(bookings);
        }

    }
}
