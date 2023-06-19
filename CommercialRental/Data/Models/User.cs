using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CommercialRental.Data.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public List<Advertisment>? Advertisments { get; set; }
        public List<RequestRent>? RequestRents { get; set; }
    }
}
