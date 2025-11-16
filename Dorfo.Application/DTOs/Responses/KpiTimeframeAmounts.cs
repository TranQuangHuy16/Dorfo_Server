using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class KpiTimeframeAmounts
    {
        public decimal Today { get; set; }
        public decimal ThisMonth { get; set; }
        public decimal ThisYear { get; set; }
    }
}
