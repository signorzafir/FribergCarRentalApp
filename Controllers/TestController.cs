using FribergCarRentalApp.Data;
using FribergCarRentalApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalApp.Controllers
{
    public class TestController : Controller
    {
        //private readonly ICustomerRepository customerRepository;

        //public TestController(ICustomerRepository customerRepository)
        //{
        //    this.customerRepository = customerRepository;
        //}
        //[Route("Test/Customer")]
        //public IActionResult testCustomerRepository()
        //{
        //    customerRepository.AddCustomer(new Customer { Name = "Johny Bravo", Email = "sigijohny@my.com", Password = "1234567" });


        //    var cust = customerRepository.GetAllCustomers();
        //    if (cust != null)
        //    {
        //        foreach (var item in cust)
        //        {
        //            Console.WriteLine(item.Name);
        //        }
        //    }
        //    else
        //    {
        //        return Content("not Found");
        //    }


        //    var customerToUpdate = customerRepository.GetCustomerByIdOrName("Johny Bravo");
        //    customerToUpdate.Name = "John Doe";
        //    customerRepository.UpdateCustomer(customerToUpdate);
        //    Console.WriteLine($"Customer's new name is {customerToUpdate.Name}");


        //    var cust = customerRepository.GetCustomerByIdOrName("John Doe");
        //    customerRepository.DeleteCustomer(cust);


        //    return Content("Test Method run completed");
        //}
        // GET: TestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
