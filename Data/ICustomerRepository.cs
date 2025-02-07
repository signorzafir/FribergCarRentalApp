using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Data
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer? GetCustomerByIdOrName(int id);
        Customer? GetCustomerByIdOrName(string name);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
