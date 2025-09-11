namespace Dorfo.Application.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public UserNotFoundException(string message)
            : base(404, message) { }
    }
}
