using HealthTracker.Models.DTOs.Coach;

namespace HealthTracker.Services.Interfaces
{
    public interface ICoachService
    {
        public Task<List<GetCoachDataDTO>> GetAllInactiveCoach();
    }
}
