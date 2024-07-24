using HealthTracker.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class UserPreferenceRepository : AbstractRepository<int, UserPreference>
    {
        public UserPreferenceRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
