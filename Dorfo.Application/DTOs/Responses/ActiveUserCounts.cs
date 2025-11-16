using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class ActiveUserCounts
    {
        public int Last24h { get; set; }
        public int Last7d { get; set; }
        public int Last30d { get; set; }
    }
}
