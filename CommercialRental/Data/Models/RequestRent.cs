using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CommercialRental.Data.Models
{
    public class RequestRent
    {
        public int Id { get; set; }
        public int? AdvertismentId { get; set; }
        public Advertisment? RequestAdvertisment { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
