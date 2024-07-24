using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class TargetAlreadyExistsException : Exception
    {
        public string msg;
        public TargetAlreadyExistsException()
        {
            msg = "Target already set for given date!";
        }

        public override string Message => msg;
    }
}