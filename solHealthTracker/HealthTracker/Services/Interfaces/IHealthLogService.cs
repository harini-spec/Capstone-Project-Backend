using HealthTracker.Models.DTOs.HealthLog;

namespace HealthTracker.Services.Interfaces
{
    public interface IHealthLogService
    {
        public Task<HealthLogOutputDTO> AddHealthLog(HealthLogInputDTO healthLogInputDTO, int UserId);
    }
}
