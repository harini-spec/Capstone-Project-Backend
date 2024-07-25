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

        public NoItemsFoundException(string msg)
        {
            this.msg = msg;
        }

        public override string Message => msg;
    }
}