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
            CreateMap<User, UserResponse>()
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender == true ? "Male" : "Female"));
            CreateMap<UserResponse, User>();
            CreateMap<User, UserCreateResponse>();
            // Merchant
            CreateMap<Merchant, MerchantResponse>();
            CreateMap<MerchantResponse, Merchant>();
            CreateMap<MerchantRequest, Merchant>();
        }
    }
}
