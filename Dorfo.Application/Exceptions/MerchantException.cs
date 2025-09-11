namespace Dorfo.Application.Exceptions
{
    public class MerchantException : AppException
    {
        public MerchantException(string message)
        : base(400, message)
        {
        }

        public MerchantException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        // Exception Not Found Id
        public static MerchantException NotFound(Guid id) =>
            new MerchantException(404, $"Merchant with ID '{id}' was not found.");
    }
}
