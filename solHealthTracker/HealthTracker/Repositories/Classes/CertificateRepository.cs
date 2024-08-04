using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Interfaces;

namespace HealthTracker.Repositories.Classes
{
    public class CertificateRepository : AbstractRepository<int, CoachCertificate>
    {
        public CertificateRepository(HealthTrackerContext context) : base(context)
        {
        }
    }
}
