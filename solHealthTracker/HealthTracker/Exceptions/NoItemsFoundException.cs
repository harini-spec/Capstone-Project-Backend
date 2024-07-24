using System.Runtime.Serialization;

namespace HealthTracker.Exceptions
{
    public class NoItemsFoundException : Exception
    {
        public string msg;
        public NoItemsFoundException()
        {
            msg = "No Items found!";
        }

        public override string Message => msg;
    }
}