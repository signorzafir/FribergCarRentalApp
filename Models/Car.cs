using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string FullName => $"{Make} {Model}";

    }
}
