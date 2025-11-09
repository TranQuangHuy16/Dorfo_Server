using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? FcmToken { get; set; }

        public UserRoleEnum Role { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public LoyaltyAccount? LoyaltyAccount { get; set; }
    }
}
