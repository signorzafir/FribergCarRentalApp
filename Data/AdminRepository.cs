using FribergCarRentalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalApp.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly RentalAppDbContext rentalAppDbContext;

        public AdminRepository(RentalAppDbContext rentalAppDbContext)
        {
            this.rentalAppDbContext = rentalAppDbContext;
        }

        public Admin? GetAdminByEmail(string email)
        {
            return rentalAppDbContext.Admins.FirstOrDefault(a => a.Email == email);
        }
    }
}
