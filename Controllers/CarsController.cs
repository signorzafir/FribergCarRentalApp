using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentalApp.Data;
using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarRepository carRepository;

        public CarsController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }
        
        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Index", "Cars") });
            }

            var cars = carRepository.GetAllCars();
            return View(cars);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Details", "Cars") });
            }

            if (id == null)
            {
                return NotFound();
            }

            var car = carRepository.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Create", "Cars") });
            }

            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Make,Model,Year,PricePerDay,ImageUrls")] Car car)
        {
            if (ModelState.IsValid)
            {
                carRepository.AddCar(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Edit", "Cars") });
            }

            if (id == null)
            {
                return NotFound();
            }

            var car = carRepository.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,PricePerDay,ImageUrls")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carRepository.UpdateCar(car);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Index", "Admin", new { returnUrl = Url.Action("Delete", "Cars") });
            }

            if (id == null)
            {
                return NotFound();
            }

            var car = carRepository.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = carRepository.GetCarById(id);
            if (car != null)
            {
                carRepository.DeleteCar(car);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return carRepository.GetAllCars().Any(e => e.Id == id);
        }
    }
}
