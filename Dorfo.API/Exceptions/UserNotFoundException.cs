namespace Dorfo.API.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public UserNotFoundException(string message)
            : base(404, message) { }
    }
}
