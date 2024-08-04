using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Interfaces;

namespace HealthTracker.Repositories.Classes
{
    public class CertificateRepository : AbstractRepository<int, Certificate>
    {
        public CertificateRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
