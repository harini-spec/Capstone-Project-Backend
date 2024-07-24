using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException()
        {
        }

        public UnauthorizedUserException(string? message) : base(message)
        {
        }
    }
}