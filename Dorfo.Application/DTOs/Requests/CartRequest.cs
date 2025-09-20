using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class CartRequest
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public Guid? MerchantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<CartItemRequest> Items { get; set; } = new();
    }
}
