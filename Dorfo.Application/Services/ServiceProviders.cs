using Dorfo.Application.Interfaces.Services;

public class ServiceProviders : IServiceProviders
{
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;
    private readonly IMerchantService _merchantService;

    public ServiceProviders(IUserService userService, IOtpService otpService, IMerchantService merchantService)
    {
        _userService = userService;
        _otpService = otpService;
        _merchantService = merchantService;
    }

    public IUserService UserService => _userService;
    public IOtpService OtpService => _otpService;
    public IMerchantService MerchantService => _merchantService;
}
