using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace FribergCarRentalApp.Data
{
    public class RentalAppDbContext : DbContext
    {
        public RentalAppDbContext(DbContextOptions<RentalAppDbContext> options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

    }
}
