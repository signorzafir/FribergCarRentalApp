using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Data
{
    public interface IAdminRepository
    {
        Admin? GetAdminByEmail(string email);
    }
}
