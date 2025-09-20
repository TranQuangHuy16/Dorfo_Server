using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class MerchantSettingRequest
    {
        public bool SupportsScheduling { get; set; }
        public int DeliveryRadiusMeters { get; set; }
    }
}
