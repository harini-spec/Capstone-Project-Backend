using ATMApplication.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class TargetRepository : AbstractRepository<int, Target>
    {
        public TargetRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
