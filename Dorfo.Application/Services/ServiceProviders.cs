using Dorfo.Application.Interfaces.Services;

public class ServiceProviders : IServiceProviders
{
    private readonly IUserService _userService;
    private readonly IOtpService _otpService;

    public ServiceProviders(IUserService userService, IOtpService otpService)
    {
        _userService = userService;
        _otpService = otpService;
    }

    public IUserService UserService => _userService;
    public IOtpService OtpService => _otpService;
}
