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
    public class BookingsController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ICarRepository carRepository;
        private readonly ICustomerRepository customerRepository;

        public BookingsController(IBookingRepository bookingRepository, 
                                  ICarRepository carRepository,
                                  ICustomerRepository customerRepository)
        {
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
            this.customerRepository = customerRepository;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var Bookings = bookingRepository.GetAllBookings().Include(b => b.Car).Include(b => b.Customer).ToList();
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars().Select(c => new { c.Id, FullName = c.Make + " - " + c.Model }), "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email");
            return View(Bookings);

        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
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

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Make");
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars().Select(c=> new {c.Id, FullName = c.Make + " - " + c.Model}), "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,CustomerId,StartDate,EndDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                bookingRepository.AddBooking(booking);
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars(), "Id", "Make", booking.CarId);
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email", booking.CustomerId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
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
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars(), "Id", "Make", booking.CarId);
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email", booking.CustomerId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,CustomerId,StartDate,EndDate")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bookingRepository.UpdateBooking(booking);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["CarId"] = new SelectList(carRepository.GetAllCars(), "Id", "Make", booking.CarId);
            ViewData["CustomerId"] = new SelectList(customerRepository.GetAllCustomers(), "Id", "Email", booking.CustomerId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int id)
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

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = bookingRepository.GetBookingById(id);
            if (booking != null)
            {
               bookingRepository.DeleteBooking(booking);
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return bookingRepository.GetAllBookings().Any(e => e.Id == id);
        }
    }
}
