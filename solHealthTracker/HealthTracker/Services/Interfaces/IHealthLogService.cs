using HealthTracker.Models.DTOs.HealthLog;

namespace HealthTracker.Services.Interfaces
{
    public interface IHealthLogService
    {
        public Task<AddHealthLogOutputDTO> AddHealthLog(AddHealthLogInputDTO healthLogInputDTO, int UserId);
        public Task<GetHealthLogOutputDTO> GetHealthLog(int PrefId, int UserId);
    }
}
