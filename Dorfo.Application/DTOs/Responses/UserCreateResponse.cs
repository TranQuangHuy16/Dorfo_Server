using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class UserCreateResponse
    {
        public Guid UserId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; } = null!; // đã hash
        public int Role { get; set; }
    }
}
