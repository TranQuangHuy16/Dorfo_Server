using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class UserUpdateRequest
    {
        public string? DisplayName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
