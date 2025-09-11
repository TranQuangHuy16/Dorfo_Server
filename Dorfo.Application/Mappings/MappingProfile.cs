using AutoMapper;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dorfo.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();
            // Merchant
            CreateMap<Merchant, MerchantResponse>();
            CreateMap<MerchantResponse, Merchant>();
            CreateMap<MerchantRequest, Merchant>();
        }
    }
}
