using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] 
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")] 
        public string Password { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
