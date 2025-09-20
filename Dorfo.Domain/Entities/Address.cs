using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public Guid? UserId { get; set; }

        public string? AddressLabel { get; set; } 
        public string Street { get; set; } = null!; 
        public string? Ward { get; set; }
        public string? District { get; set; } 
        public string? City { get; set; } 
        public string? Country { get; set; } 

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
    }

}
