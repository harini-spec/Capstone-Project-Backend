using ATMApplication.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class IdealDataRepository : AbstractRepository<int, IdealData>
    {
        public IdealDataRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
