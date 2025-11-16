using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class KpiTimeframeCounts
    {
        public int Today { get; set; }
        public int ThisMonth { get; set; }
        public int ThisYear { get; set; }
    }
}
