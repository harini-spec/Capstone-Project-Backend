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

        public EntityNotFoundException(string msg)
        {
            this.msg = msg;
        }

        public override string Message => msg;
    }
}