using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class OtpCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PhoneNumber { get; set; } = default!;
        public string Code { get; set; } = default!;
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
