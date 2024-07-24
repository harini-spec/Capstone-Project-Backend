using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Target;

namespace HealthTracker.Services.Interfaces
{
    public interface ITargetService
    {
        public Task<string> AddTarget(TargetInputDTO targetInputDTO, int UserId);
        public Task<TargetOutputDTO> GetTodaysTarget(string MetricType, int UserId);
        public Task<string> UpdateTarget(UpdateTargetInputDTO updateTargetInputDTO, int UserId);
    }
}
