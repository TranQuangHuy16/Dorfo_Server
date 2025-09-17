using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class AddCartItemRequest
    {
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal PriceAtAdd { get; set; }
        public List<CartItemOptionRequest> Options { get; set; } = new();
        //public string? OptionsJson { get; set; }
        //public DateTime? ScheduledFor { get; set; }
    }

    public class AddCartItemsRequest
    {
        //public Guid UserId { get; set; }
        public Guid MerchantId { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal Discount { get; set; }
        public List<AddCartItemRequest> Items { get; set; } = new();

    }

    public class CartItemOptionRequest
    {
        public Guid OptionId { get; set; }
        public List<Guid> SelectedValueIds { get; set; } = new();
    }

    //public class CartItemOptionRequest
    //{
    //    public Guid OptionId { get; set; }
    //    public string OptionName { get; set; } = null!;
    //    public List<CartItemOptionValueRequest> SelectedValues { get; set; } = new();
    //}

    //public class CartItemOptionValueRequest
    //{
    //    public Guid OptionValueId { get; set; }
    //    public string? ValueName { get; set; }
    //    public decimal PriceDelta { get; set; }
    //}
}
