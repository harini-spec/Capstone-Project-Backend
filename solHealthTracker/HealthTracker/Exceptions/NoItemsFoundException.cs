using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class NoItemsFoundException : Exception
    {
        public NoItemsFoundException()
        {
        }

        public NoItemsFoundException(string? message) : base(message)
        {
        }
    }
}