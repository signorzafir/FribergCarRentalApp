using FribergCarRentalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalApp.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly RentalAppDbContext rentalAppDbContext;

        public BookingRepository(RentalAppDbContext rentalAppDbContext)
        {
            this.rentalAppDbContext = rentalAppDbContext;
        }
        public void AddBooking(Booking booking)
        {
            rentalAppDbContext.Bookings.Add(booking);
            rentalAppDbContext.SaveChanges();
        }

        public void DeleteBooking(Booking booking)
        {
            rentalAppDbContext.Remove(booking);
            rentalAppDbContext.SaveChanges();
        }

        public IQueryable<Booking> GetAllBookings()
        {
            return rentalAppDbContext.Bookings.OrderBy(b => b.Customer.Name);
        }

        public Booking? GetBookingById(int id)
        {
            return rentalAppDbContext.Bookings.Include(b => b.Customer).Include(b => b.Car).FirstOrDefault(b => b.Id == id);
        }

        public void UpdateBooking(Booking booking)
        {
            rentalAppDbContext.Update(booking);
            rentalAppDbContext.SaveChanges();
        }
    }
}
