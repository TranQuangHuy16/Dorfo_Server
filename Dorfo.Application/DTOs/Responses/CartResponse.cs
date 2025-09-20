using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class CartResponse
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        //public Guid MerchantId { get; set; }
        public MerchantInfo Merchant { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        public List<CartItemResponse> Items { get; set; } = new();
    }

    public class MerchantInfo
    {
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } = null!;

    }
}
