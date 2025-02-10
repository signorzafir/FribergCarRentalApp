using FribergCarRentalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalApp.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RentalAppDbContext rentalAppDbContext;

        public CustomerRepository(RentalAppDbContext rentalAppDbContext)
        {
            this.rentalAppDbContext = rentalAppDbContext;
        }
        public void AddCustomer(Customer customer)
        {
            rentalAppDbContext.Customers.Add(customer);
            rentalAppDbContext.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            rentalAppDbContext.Customers.Remove(customer);
            rentalAppDbContext.SaveChanges();
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            return rentalAppDbContext.Customers.OrderBy(c => c.Name);
        }

        public Customer? GetCustomerByIdOrName(int id)
        {
            return rentalAppDbContext.Customers
                .Include(c => c.Bookings)
                .FirstOrDefault(c => c.Id == id);
        }

        public Customer? GetCustomerByIdOrName(string name)
        {
            return rentalAppDbContext.Customers.FirstOrDefault(c => c.Name == name);
        }

        public void UpdateCustomer(Customer customer)
        {
            rentalAppDbContext.Customers.Update(customer);
            rentalAppDbContext.SaveChanges();
        }
    }
}
