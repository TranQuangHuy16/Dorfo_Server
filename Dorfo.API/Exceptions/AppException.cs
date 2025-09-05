using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Exceptions
{
    public abstract class AppException : Exception
    {
        public int ErrorCode { get; }

        protected AppException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
