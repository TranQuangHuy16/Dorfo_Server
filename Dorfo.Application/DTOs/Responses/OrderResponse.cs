using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderRef { get; set; } = null!;
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } = null!;
        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }

    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public List<OrderItemOptionResponse> Options { get; set; } = new();
    }

    public class OrderItemOptionResponse
    {
        public Guid OrderItemOptionId { get; set; }
        public string OptionName { get; set; } = null!;
        public List<OrderItemOptionValueResponse> Values { get; set; } = new();
    }

    public class OrderItemOptionValueResponse
    {
        public Guid OrderItemOptionValueId { get; set; }
        public string ValueName { get; set; } = null!;
        public decimal PriceDelta { get; set; }
    }
}
