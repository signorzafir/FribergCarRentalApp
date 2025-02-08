using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required] public int CarId { get; set; }
        [Required] public int CustomerId { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set;}

        [ValidateNever]
        public Car Car { get; set; }

        [ValidateNever]
        public Customer Customer { get; set; }
    }
}
