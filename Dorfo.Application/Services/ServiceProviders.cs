using Dorfo.Application.Interfaces.Services;

public class ServiceProviders : IServiceProviders
{
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;
    private readonly IMerchantService _merchantService;
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;
    //private readonly IOrderService _orderService;
    private readonly IMerchantOpeningDayService _merchantOpeningDayService;
    private readonly IMenuCategoryService _menuCategoryService;
    private readonly IMenuItemService _menuItemService;
    private readonly IMenuItemOptionService _menuItemOptionService;
    private readonly IMenuItemOptionValueService _menuItemOptionValueService;
    private readonly IShipperService _shipperService;
    private readonly IAddressService _addressService;
    private readonly IOrderService _orderService;
    private readonly IMerchantCategoryService _merchantCategoryService;
    private readonly IReviewService _reviewService;
    private readonly IFavoriteShopService _favoriteShopService;
    private readonly IDashboardService _dashboardService;

    public ServiceProviders(IUserService userService,
        IOtpService otpService,
        IMerchantService merchantService,
        IAuthService authService,
        IMerchantOpeningDayService merchantOpeningDayService,
        IMenuCategoryService menuCategoryService,
        IMenuItemService menuItemService,
        IMenuItemOptionService menuItemOptionService,
        IMenuItemOptionValueService menuItemOptionValueService,
        IShipperService shipperService,
        ICartService cartService,
        IAddressService addressService,
        IOrderService orderService,
        IMerchantCategoryService merchantCategoryService,
        IReviewService reviewService,
        IFavoriteShopService favoriteShopService,
        IDashboardService dashboardService)
    {
        _userService = userService;
        _otpService = otpService;
        _merchantService = merchantService;
        _authService = authService;
        _cartService = cartService;
        _merchantOpeningDayService = merchantOpeningDayService;
        _menuCategoryService = menuCategoryService;
        _menuItemService = menuItemService;
        _menuItemOptionService = menuItemOptionService;
        _menuItemOptionValueService = menuItemOptionValueService;
        _shipperService = shipperService;
        _addressService = addressService;
        _orderService = orderService;
        _merchantCategoryService = merchantCategoryService;
        _reviewService = reviewService;
        _favoriteShopService = favoriteShopService;
        _dashboardService = dashboardService;
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
    public ICartService CartService => _cartService;
    public IAddressService AddressService => _addressService;
    public IOrderService OrderService => _orderService;
    public IMerchantCategoryService MerchantCategoryService => _merchantCategoryService;
    public IReviewService ReviewService => _reviewService;
    public IFavoriteShopService FavoriteShopService => _favoriteShopService;
    public IDashboardService DashboardService => _dashboardService;
}
