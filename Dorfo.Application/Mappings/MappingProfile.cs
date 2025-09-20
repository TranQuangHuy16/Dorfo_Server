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
            CreateMap<Merchant, MerchantResponse>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.MerchantAddress))
                .ForMember(dest => dest.Setting, opt => opt.MapFrom(src => src.MerchantSetting));

            //CreateMap<MerchantResponse, Merchant>()
            //    .ForMember(dest => dest.MerchantAddress, opt => opt.MapFrom(src => src.Address))
            //    .ForMember(dest => dest.MerchantSetting, opt => opt.MapFrom(src => src.Setting));

            CreateMap<MerchantAddress, MerchantAddressResponse>();
            //CreateMap<MerchantAddressResponse, MerchantAddress>();

            CreateMap<MerchantSetting, MerchantSettingResponse>();
            //CreateMap<MerchantSettingResponse, MerchantSetting>();



            CreateMap<MerchantRequest, Merchant>()
                .ForMember(dest => dest.MerchantAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.MerchantSetting, opt => opt.MapFrom(src => src.Setting));

            CreateMap<MerchantAddressRequest, MerchantAddress>();
            CreateMap<MerchantSettingRequest, MerchantSetting>();

            // MerchantOpeningDay
            CreateMap<MerchantOpeningDayRequest, MerchantOpeningDay>();
            CreateMap<MerchantOpeningDay, MerchantOpeningDayResponse>();

            // MenuCategory
            CreateMap<MenuCategoryRequest, MenuCategory>();
            CreateMap<MenuCategory, MenuCategoryResponse>();

            // MenuItem
            CreateMap<MenuItemRequest, MenuItem>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<MenuItemOptionRequest, MenuItemOption>()
                .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Values));

            CreateMap<MenuItemOptionValueRequest, MenuItemOptionValue>();


            CreateMap<MenuItem, MenuItemResponse>();
            CreateMap<MenuItemOption, MenuItemOptionResponse>();
            CreateMap<MenuItemOptionValue, MenuItemOptionValueResponse>();

            // Shipper
            CreateMap<ShipperRequest, Shipper>();
            CreateMap<Shipper, ShipperResponse>();
        }
    }
}
