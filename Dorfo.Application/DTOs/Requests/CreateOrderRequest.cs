using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class CreateOrderRequest
    {
        //public Guid UserId { get; set; }
        public Guid CartId { get; set; }
        public Guid? DeliveryAddressId { get; set; }
        //public string? DeliveryPoint { get; set; }
        //public int PaymentMethodId { get; set; }
        public string? Notes { get; set; }
    }
}
