using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class PaymentResponse
    {
        public string Provider { get; set; } = null!;
        public string ProviderReference { get; set; } = null!;
        public string? PaymentUrl { get; set; }  // nếu cổng trả về link
        public string? QrImage { get; set; }    // nếu cổng trả về QR base64
        public decimal Amount { get; set; }
        public string OrderRef { get; set; } = null!;
    }

}
