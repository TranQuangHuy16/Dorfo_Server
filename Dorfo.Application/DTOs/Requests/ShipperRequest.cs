namespace Dorfo.Application
{
    public class ShipperRequest
    {
        public Guid UserId { get; set; }
        public Guid MerchantId { get; set; }
        public string? VehicleType { get; set; }
        public string? LicensePlate { get; set; }

        public string? CccdFrontUrl { get; set; }
        public string? CccdBackUrl { get; set; }

        public bool IsOnline { get; set; } = false;
    }
}