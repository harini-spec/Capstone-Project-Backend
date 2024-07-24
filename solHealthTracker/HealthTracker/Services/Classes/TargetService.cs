using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class TargetService : ITargetService
    {
        private readonly IRepository<int, Target> _TargetRepository;
        private readonly IMetricService _MetricService;

        public TargetService(IRepository<int, Target> TargetRepository, IMetricService metricService)
        {
            _TargetRepository = TargetRepository;
            _MetricService = metricService;
        }

        public async Task<string> AddTarget(TargetInputDTO targetInputDTO, int UserId)
        {
            try
            {
                int prefId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId(targetInputDTO.MetricType, UserId);
                Target target = new Target();
                target.PreferenceId = prefId;
                target.Created_at = DateTime.Now;
                target.Updated_at = DateTime.Now;
                target.TargetMaxValue = targetInputDTO.TargetMaxValue;
                target.TargetMinValue = targetInputDTO.TargetMinValue;
                target.StartDate = targetInputDTO.StartDate;
                target.EndDate = targetInputDTO.EndDate;
                target.TargetStatus = Models.ENUMs.TargetStatusEnum.TargetStatus.Ongoing;
                await _TargetRepository.Add(target);
                return "Successfully added!";
            }
            catch(Exception)
            { throw; }
        }

        public async Task<TargetOutputDTO> GetTarget(string MetricType, int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
