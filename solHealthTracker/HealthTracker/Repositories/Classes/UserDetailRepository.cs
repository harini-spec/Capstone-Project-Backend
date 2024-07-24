using HealthTracker.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class UserDetailRepository : AbstractRepository<int, UserDetail>
    {
        public UserDetailRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
