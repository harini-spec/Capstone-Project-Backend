using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class UnableToRegisterException : Exception
    {

        public UnableToRegisterException(string? message) : base(message)
        {
        }
    }
}