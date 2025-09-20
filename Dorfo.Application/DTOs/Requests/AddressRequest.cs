using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class AddressRequest
    {
        public string? AddressLabel { get; set; }
        public string Street { get; set; } = null!;
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public bool IsDefault { get; set; }
    }
}
