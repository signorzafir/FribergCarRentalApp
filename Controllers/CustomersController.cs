using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentalApp.Data;
using FribergCarRentalApp.Models;
using FribergCarRentalApp.ViewModels;
using Newtonsoft.Json;

namespace FribergCarRentalApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        private readonly IBookingRepository bookingRepository;
        private readonly ICarRepository carRepository;


        public CustomersController(ICustomerRepository customerRepository,
                                   IBookingRepository bookingRepository,
                                   ICarRepository carRepository)
        {
            this.customerRepository = customerRepository;
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = customerRepository.GetAllCustomers().Include(c => c.Bookings);
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerByIdOrName(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Create", "Customers") });
            }
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] Customer customer)
        {


            if (ModelState.IsValid)
            {
                customerRepository.AddCustomer(customer);

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerByIdOrName(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customerRepository.UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerByIdOrName(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = customerRepository.GetCustomerByIdOrName(id);
            if (customer != null)
            {
                if (customer.Bookings.Any())
                {
                    ModelState.AddModelError("", "Cannot delete a customer with bookings.");
                    return View(customer);
                }
                else
                {
                    customerRepository.DeleteCustomer(customer);

                }

            }

            return RedirectToAction(nameof(Index));
        }

        //Get: /Customers/Login
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //Post: /Customers/Login
        [HttpPost]
        public IActionResult Login(CustomerLoginViewModel customerLoginVM, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                var customer = customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Email == customerLoginVM.Email && c.Password == customerLoginVM.Password);
                if (customer == null)
                {
                    ViewData["Message"] = "Invalid user id or password.";
                    ViewData["ReturnUrl"] = returnUrl;
                    return View(customerLoginVM);
                }
                HttpContext.Session.SetString("CustomerName", customer.Name);
                HttpContext.Session.SetInt32("CustomerId", customer.Id);
                //string? ReturnUrl = TempData["ReturnUrl"] as string;
                //return !string.IsNullOrEmpty(ReturnUrl) ? Redirect(ReturnUrl) : RedirectToAction("MyBookings", "Customers");
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("UserHome", "Customers");
                }

            }
            return View(customerLoginVM);
        }

        public IActionResult MyBookings()
        {

            if (TempData.Peek("Message") != null)
            {

                ViewBag.Message = TempData["Message"];

            }
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login");
            }
            var customer = customerRepository.GetCustomerByIdOrName(customerId ?? 0);
            if (customer == null)
            {
                return RedirectToAction("ErrorView");
            }

            var bookings = bookingRepository.GetAllBookings()
                                            .Where(b => b.CustomerId == customerId)
                                            .Include(b => b.Car);


            ViewBag.LoggedInCustomer = HttpContext.Session.GetString("CustomerName");

            return View(bookings);
        }

        public IActionResult CancelBooking(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = bookingRepository.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            if (booking.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("EndDate", "Can not Delete past bookings!");
                return View(booking);
            }
            //ViewBag.CarName = booking.Car.FullName;
            return View(booking);

        }
        [HttpPost]
        public IActionResult CancelBooking(Booking booking)
        {
            if (booking == null)
            {
                return NotFound();
            }
            //ViewBag.CarName = booking.Car.FullName;
            if (booking.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("EndDate", "Can not Delete past bookings!");
                return View(booking);
            }
            bookingRepository.DeleteBooking(booking);
            return RedirectToAction("MyBookings");
        }

        public IActionResult RegisterAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterAccount(Customer customer)
        {
            if (customer == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var DuplicateCustomer = customerRepository.GetAllCustomers().Where(c => c.Email == customer.Email);
                if (DuplicateCustomer != null)
                {
                    ModelState.AddModelError("Email", "This Email is already registered");
                }
                customerRepository.AddCustomer(customer);
                TempData["RegistraionSuccessful"] = "Account registered Successfully!";
                return RedirectToAction("Login");
            }
            return View(customer);
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("CustomerName");
            HttpContext.Session.Remove("CustomerId");

            return RedirectToAction("Login");
        }

        private bool CustomerExists(int id)
        {
            return customerRepository.GetAllCustomers().Any(e => e.Id == id);
        }

        public IActionResult CreateBooking(int carId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                TempData["carIdAfterLogin"] = carId;
                return RedirectToAction("Login","Customers", new { returnUrl = Url.Action("CreateBooking", "Customers") });
            }
            if (TempData.Peek("carIdAfterLogin") != null) 
            {
                carId = Convert.ToInt32( TempData["carIdAfterLogin"]);
            }
            var booking = new Booking
            {
                CarId = carId
            };

            return View(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking(Booking booking)
        {

            int customerId =
            Convert.ToInt32(HttpContext.Session.GetInt32("CustomerId"));

            booking.CustomerId = customerId;

            if (HttpContext.Session.GetInt32("CustomerId") == null)
            {
                TempData["Booking"] = JsonConvert.SerializeObject(booking);
                //TempData["Car-id"] = carId;
                //TempData["Customer-id"] = customerId;
                //TempData["Start-date"] = startdate;
                //TempData
                TempData["ReturnUrl"] = "ContinueCreateBooking";
                return RedirectToAction("Login");
            }

            if (booking.StartDate >= booking.EndDate || booking.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError("StartDate", "Invalid date sequence!");
                ModelState.AddModelError("EndDate", "Invalid date sequence!");

                return View(booking);
            }
            if (ModelState.IsValid)
            {

                bookingRepository.AddBooking(booking);
                TempData["Message"] = "Your Booking is Confirmed now!";
                return RedirectToAction("MyBookings");
            }

            return RedirectToAction("MyBookings");
        }
        public ActionResult ContinueCreateBooking(int Id)
        {

            return RedirectToAction("MyBookings");
        }
        public IActionResult UserHome()
        {
            var customerName = HttpContext.Session.GetString("CustomerName");
            if (customerName == null)
            {
                return RedirectToAction("Login");
            }
            var Cars = carRepository.GetAllCars();
            ViewBag.LoggedInCustomer = customerName;
            return View(Cars);
        }







    }
}
