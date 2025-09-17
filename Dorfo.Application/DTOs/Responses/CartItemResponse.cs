using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class CartItemResponse
    {
        public Guid CartItemId { get; set; }
        public Guid MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtAdd { get; set; }
        public List<CartItemOptionResponse> Options { get; set; } = new();
        //public string? OptionsJson { get; set; }
        //public DateTime? ScheduledFor { get; set; }
    }

    public class CartItemOptionResponse
    {
        public Guid OptionId { get; set; }
        public string OptionName { get; set; } = null!;
        public List<CartItemOptionValueResponse> SelectedValues { get; set; } = new();
    }

    public class CartItemOptionValueResponse
    {
        public Guid OptionValueId { get; set; }
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }
    }
}
