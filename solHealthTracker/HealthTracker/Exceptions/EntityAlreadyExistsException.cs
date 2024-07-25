using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}