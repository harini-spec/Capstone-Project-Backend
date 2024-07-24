using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class UserNotActiveException : Exception
    {
        public UserNotActiveException()
        {
        }

        public UserNotActiveException(string? message) : base(message)
        {
        }
    }
}