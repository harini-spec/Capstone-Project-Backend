using HealthTracker.Models.DTOs.HealthLog;

namespace HealthTracker.Services.Interfaces
{
    public interface IHealthLogService
    {
        public Task<AddHealthLogOutputDTO> AddHealthLog(AddHealthLogInputDTO healthLogInputDTO, int UserId, bool isGFitData);
        public Task<GetHealthLogOutputDTO> GetHealthLog(int PrefId, int UserId);
        public Task<AddHealthLogOutputDTO> UpdateHealthLog(int HealthLogId, float value, int UserId, bool isGFitData);
        public Task<string> AddHealthLogDataFromGoogleFit(List<AddHealthLogFromGoogleFitInputDTO> addHealthLogFromGoogleFitInputDTO, int UserId);
    }
}
