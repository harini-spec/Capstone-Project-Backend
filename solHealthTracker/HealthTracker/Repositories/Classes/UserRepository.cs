using HealthTracker.Repositories;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Repositories.Classes
{
    public class UserRepository : AbstractRepository<int, User>
    {
        public UserRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
