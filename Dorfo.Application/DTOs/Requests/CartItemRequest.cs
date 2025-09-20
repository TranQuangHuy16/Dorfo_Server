using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class CartItemRequest
    {
        public Guid CartItemId { get; set; }
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtAdd { get; set; }
        public string? OptionsJson { get; set; }
        public DateTime? ScheduledFor { get; set; }
    }

}
