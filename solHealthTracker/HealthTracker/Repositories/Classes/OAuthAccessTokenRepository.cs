using HealthTracker.Models;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Repositories.Classes
{
    public class OAuthAccessTokenRepository : AbstractRepository<int, OAuthAccessTokenModel>
    {
        public OAuthAccessTokenRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
