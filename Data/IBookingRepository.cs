using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Data
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking? GetBookingById(int id);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(Booking booking);

    }
}
