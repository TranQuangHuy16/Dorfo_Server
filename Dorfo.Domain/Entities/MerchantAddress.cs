using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MerchantAddress
    {
        public Guid MerchantAddressId { get; set; }
        public Guid MerchantId { get; set; }
        public string? StreetNumber { get; set; }   
        public string? StreetName { get; set; }     
        public string? Ward { get; set; }           
        public string? District { get; set; }       
        public string? City { get; set; }           
        public Merchant Merchant { get; set; } = null!;
    }

}
