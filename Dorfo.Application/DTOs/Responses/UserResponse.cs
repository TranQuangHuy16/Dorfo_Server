using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class UserResponse
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
