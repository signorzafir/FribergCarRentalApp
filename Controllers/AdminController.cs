using FribergCarRentalApp.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = rentalAppDbContext.Admins.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (admin == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View("Index");
            }

            TempData["Message"] = "Welcome to Admin Page!";
            return RedirectToAction("Dashboard");


        }
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
