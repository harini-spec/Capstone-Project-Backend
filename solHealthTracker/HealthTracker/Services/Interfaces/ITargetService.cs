using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs.Target;

namespace HealthTracker.Services.Interfaces
{
    public interface ITargetService
    {
        public Task<string> AddTarget(TargetInputDTO targetInputDTO);
        public Task<TargetOutputDTO> GetTodaysTarget(int PrefId, int UserId);
        public Task UpdateTargetRepo(Target target);
        public Task<string> UpdateTarget(UpdateTargetInputDTO updateTargetInputDTO);
        public Task<Target> GetTargetById(int TargetId);
        public Task<string> calculateTargetStatus(AddHealthLogInputDTO healthLogInputDTO, int UserId);

        public Task<string> DeleteTargetById(int TargetId);
        public Task<TargetOutputDTO> GetTargetDTOById(int TargetId);
        public Task<List<TargetOutputDTO>> GetTargetsOfPreferenceId(int prefId);

    }
}
