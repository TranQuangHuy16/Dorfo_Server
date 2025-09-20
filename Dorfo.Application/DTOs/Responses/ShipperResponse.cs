namespace Dorfo.Application
{
    public class ShipperResponse
    {
        public Guid ShipperId { get; set; }
        public Guid UserId { get; set; }
        public Guid MerchantId { get; set; }
        public string? VehicleType { get; set; }
        public string? LicensePlate { get; set; }

        public string? CccdFrontUrl { get; set; }
        public string? CccdBackUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsOnline { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}