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
        private readonly RentalAppDbContext _context;

        public CustomersController(RentalAppDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
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
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var customer = _context.Customers.Include(c => c.Bookings).FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                if (customer.Bookings.Any())
                {
                    ModelState.AddModelError("", "Cannot delete a customer with bookings.");
                    return View(customer);
                }
                else
                {
                    _context.Customers.Remove(customer);

                }

            }

            await _context.SaveChangesAsync();
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
                var customer = _context.Customers
                    .FirstOrDefault(c => c.Email == customerLoginVM.Email && c.Password == customerLoginVM.Password);
                if (customer == null) {
                    ViewData["Message"] = "Invalid user id or password.";
                    return View(customerLoginVM);
                }
                TempData["CustomerId"] = customer.Id;
                TempData["CustomerName"] = customer.Name;
                return RedirectToAction("MyBookings", "Customers");
                 
            }
            return View(customerLoginVM);
        }

        public IActionResult MyBookings()
        {
            var customerId = TempData["CustomerId"] as int?;
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login");
            }
            var bookings = _context.Bookings
                           .Where(b => b.CustomerId == customerId.Value)
                           .Include(b => b.Car)
                           .ToList();
            return View(bookings);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
