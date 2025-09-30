using Dorfo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IServiceProviders
    {
        IUserService UserService { get; }
        IOtpService OtpService { get; }
        IMerchantService MerchantService { get; }
        IAuthService AuthService { get; }
        ICartService CartService { get; }
        //IOrderService OrderService { get; }
        IMerchantOpeningDayService MerchantOpeningDayService { get; }
        IMenuCategoryService MenuCategoryService { get; }
        IMenuItemService MenuItemService { get; }
        IMenuItemOptionService MenuItemOptionService { get; }
        IMenuItemOptionValueService MenuItemOptionValueService { get; }
        IShipperService ShipperService { get; }
        IAddressService AddressService { get; }
        IOrderService OrderService { get; }
        IMerchantCategoryService MerchantCategoryService { get; }
    }
}
