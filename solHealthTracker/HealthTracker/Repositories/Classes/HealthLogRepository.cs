using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class HealthLogRepository : AbstractRepository<int, HealthLog>
    {
        public HealthLogRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
