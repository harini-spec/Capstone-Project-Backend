using HealthTracker.Models.DTOs.Target;

namespace HealthTracker.Services.Interfaces
{
    public interface ITargetService
    {
        public Task<string> AddTarget(TargetInputDTO targetInputDTO, int UserId);
        public Task<TargetOutputDTO> GetTarget(string MetricType, int UserId);
    }
}
