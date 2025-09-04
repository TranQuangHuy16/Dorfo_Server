using Dorfo.Application.Interfaces.Services;

public class ServiceProviders : IServiceProviders
{
    private readonly IUserService _userService;

    public ServiceProviders(IUserService userService)
    {
        _userService = userService;
    }

    public IUserService UserService => _userService;
}
