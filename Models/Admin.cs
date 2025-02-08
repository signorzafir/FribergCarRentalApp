using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalApp.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
