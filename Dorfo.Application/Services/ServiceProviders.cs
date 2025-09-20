using Dorfo.Application.Interfaces.Services;

public class ServiceProviders : IServiceProviders
{
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;
    private readonly IMerchantService _merchantService;
    private readonly IAuthService _authService;
    private readonly IMerchantOpeningDayService _merchantOpeningDayService;
    private readonly IMenuCategoryService _menuCategoryService;
    private readonly IMenuItemService _menuItemService;
    private readonly IMenuItemOptionService _menuItemOptionService;
    private readonly IMenuItemOptionValueService _menuItemOptionValueService;
    private readonly IShipperService _shipperService;

    public ServiceProviders(IUserService userService, 
        IOtpService otpService, 
        IMerchantService merchantService, 
        IAuthService authService, 
        IMerchantOpeningDayService merchantOpeningDayService, 
        IMenuCategoryService menuCategoryService,
        IMenuItemService menuItemService,
        IMenuItemOptionService menuItemOptionService,
        IMenuItemOptionValueService menuItemOptionValueService,
        IShipperService shipperService) 
    {
        _userService = userService;
        _otpService = otpService;
        _merchantService = merchantService;
        _authService = authService;
        _merchantOpeningDayService = merchantOpeningDayService;
        _menuCategoryService = menuCategoryService;
        _menuItemService = menuItemService;
        _menuItemOptionService = menuItemOptionService;
        _menuItemOptionValueService = menuItemOptionValueService;
        _shipperService = shipperService;
    }

    public IUserService UserService => _userService;
    public IOtpService OtpService => _otpService;
    public IMerchantService MerchantService => _merchantService;
    public IAuthService AuthService => _authService;
    public IMerchantOpeningDayService MerchantOpeningDayService => _merchantOpeningDayService;
    public IMenuCategoryService MenuCategoryService => _menuCategoryService;
    public IMenuItemService MenuItemService => _menuItemService;
    public IMenuItemOptionService MenuItemOptionService => _menuItemOptionService;
    public IMenuItemOptionValueService MenuItemOptionValueService => _menuItemOptionValueService;
    public IShipperService ShipperService => _shipperService;
}
