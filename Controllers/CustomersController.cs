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
        public IActionResult Login()
        {
            
            return View();
        }

        //Post: /Customers/Login
        [HttpPost]
        public IActionResult Login(CustomerLoginViewModel customerLoginVM)
        {
            
            if (ModelState.IsValid)
            {
                var customer = customerRepository.GetAllCustomers()
                    .FirstOrDefault(c => c.Email == customerLoginVM.Email && c.Password == customerLoginVM.Password);
                if (customer == null)
                {
                    ViewData["Message"] = "Invalid user id or password.";
                    return View(customerLoginVM);
                }
                TempData["CustomerId"] = customer.Id;
                TempData["CustomerName"] = customer.Name;
                return RedirectToAction("UserHome", "Customers");

            }
            return View(customerLoginVM);
        }

        public IActionResult MyBookings()
        {
            //var custId = TempData["CustomerId"] as int?;
            //int customerId = Convert.ToInt32(custId);
            //if (customerId == 0)
            //{
            //    return RedirectToAction("Login");
            //}
            int customerId = Convert.ToInt32(TempData.Peek("CustomerId"));
            //var customerId = TempData.Peek("CustomerId");
            if (customerId == null)
            {
                return RedirectToAction("Login");
            }
            var customer = customerRepository.GetCustomerByIdOrName(customerId);
            if (customer == null)
            {
                return RedirectToAction("ErrorView"); // Handle as needed
            }
            var bookings = customer.Bookings.ToList();
            //var bookings = customerRepository.GetCustomerByIdOrName(customerId).Bookings.ToList();
            //var bookings = customer.Bookings.ToList();
            //_context.Bookings
            //           .Where(b => b.CustomerId == customerId.Value)
            //           .Include(b => b.Car)
            //           .ToList();
            return View(bookings);
        }
        //private IActionResult CancelBooking(int id)
        //{
        //    var booking = _context.Bookings.Where(b => b.Id == id);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }
        //    _context.boo
        //}
        
        public IActionResult RegisterAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterAccount(Customer customer)
        {
            if (customer==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var DuplicateCustomer = customerRepository.GetAllCustomers().Where(c=>c.Email == customer.Email);
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
            TempData.Remove("CustomerId");
            TempData.Remove("CustomerName");

            return RedirectToAction("Login");
        }

        private bool CustomerExists(int id)
        {
            return customerRepository.GetAllCustomers().Any(e => e.Id == id);
        }

        public IActionResult CreateBooking()
        {
            ViewBag.Cars = carRepository.GetAllCars().ToList();
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars().Select(c => new { c.Id, FullName = c.Make + " - " + c.Model }), "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email");

            return View();
        }

        [HttpPost]
        public IActionResult CreateBooking(Booking booking)
        {
            var customerId = TempData["CustomerId"] as int?;
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login");
            }


            booking.CustomerId = customerId.Value;

            if (booking.StartDate >= booking.EndDate)
            {
                ModelState.AddModelError(string.Empty, "End date must be after start date.");
            }

            if (ModelState.IsValid)
            {
                bookingRepository.AddBooking(booking);
                TempData["Message"] = "Booking created successfully!";
                return RedirectToAction("MyBookings");
            }

            ViewBag.Cars = carRepository.GetAllCars().ToList();
            return View(booking);
        }
        public IActionResult UserHome() 
        {
            var customerName = TempData.Peek("CustomerName");
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
