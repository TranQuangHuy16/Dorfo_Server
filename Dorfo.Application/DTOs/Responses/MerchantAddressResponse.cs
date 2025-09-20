using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class MerchantAddressResponse
    {
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
    }
}
