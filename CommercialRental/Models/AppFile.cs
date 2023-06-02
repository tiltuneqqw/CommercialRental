namespace CommercialRental.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public int AdvertismentId { get; set; }
        public byte[] Content { get; set; }
    }
}
