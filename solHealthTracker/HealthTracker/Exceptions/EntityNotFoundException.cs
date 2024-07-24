using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string msg;
        public EntityNotFoundException()
        {
            msg = "Entity not found";
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public override string Message => msg;
    }
}