using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CommercialRental.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public List<Advertisment>? Advertisments { get; set; }
        public List<RequestRent>? RequestRents { get; set; }
    }
}
