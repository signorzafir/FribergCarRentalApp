using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.ViewModels
{
    public class CreateBookingViewModel
    {
        public int CarId { get; set; }
        public Car Car { get; set; }
        public Booking Booking { get; set; }
    }
}
