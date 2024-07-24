using ATMApplication.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class MonitorPreferenceRepository : AbstractRepository<int, MonitorPreference>
    {
        public MonitorPreferenceRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
