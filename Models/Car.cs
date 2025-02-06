using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required] public string Make { get; set; }
        [Required] public string Model { get; set; }
        public int Year { get; set; }
        [Precision(18, 2)] public decimal PricePerDay { get; set; }
        public List<string> ImageUrls { get; set; }

    }
}
