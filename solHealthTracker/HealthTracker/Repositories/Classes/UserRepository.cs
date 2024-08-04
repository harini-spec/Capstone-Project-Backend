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

        public override async Task<List<User>> GetAll()
        {
            var items = _context.Users.Include(user => user.UserDetailsForUser);
            return (List<User>)items;
        }
    }
}
