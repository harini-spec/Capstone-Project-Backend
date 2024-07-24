using ATMApplication.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class MetricRepository : AbstractRepository<int, Metric>
    {
        public MetricRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
