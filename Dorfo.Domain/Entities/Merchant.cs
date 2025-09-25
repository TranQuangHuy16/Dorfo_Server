using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Merchant
    {
        public Guid MerchantId { get; set; }
        public Guid? OwnerUserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        public decimal CommissionRate { get; set; }
        public string? ImgUrl { get; set; }
        public User? OwnerUser { get; set; }
        public MerchantSetting? MerchantSetting { get; set; }
        public MerchantAddress MerchantAddress { get; set; } = null!;
        public ICollection<MerchantOpeningDay> OpeningDays { get; set; } = new List<MerchantOpeningDay>();
        public ICollection<Shipper> Shippers { get; set; } = new List<Shipper>();
        public ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}
