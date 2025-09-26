using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class UserCreateByPhoneRequest
    {
        public string? Phone { get; set; }
        public string Password { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.CUSTOMER;
    }
}
