namespace CommercialRental.Models
{
    public class Advertisment
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string? Region { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? FullStreetName { get; set; } = string.Empty;
        public double Square { get; set; }
        public string? OfferType { get; set; } = string.Empty;
        public string? PropertyType { get; set; } = string.Empty;
        public string? OperationType { get; set; } = string.Empty;
        public string? BuildingType { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? Furniture { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.MinValue;
        public DateTime StartRentDate { get; set; } = DateTime.MinValue;
        public string Checked { get; set; } = "Не перевірено";
        public bool IsRented { get; set; }
    }
}
