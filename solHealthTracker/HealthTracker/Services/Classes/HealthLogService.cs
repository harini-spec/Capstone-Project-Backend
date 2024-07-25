using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class HealthLogService : IHealthLogService
    {
        private readonly IRepository<int, HealthLog> _HealthLogRepository;
        private readonly IRepository<int, IdealData> _IdealDataRepository;
        private readonly IMetricService _MetricService;
        private readonly ITargetService _TargetService;

        public HealthLogService(IRepository<int, HealthLog> healthLogRepository, IRepository<int, IdealData> idealDataRepository, IMetricService MetricService, ITargetService targetService)
        {
            _HealthLogRepository = healthLogRepository;
            _IdealDataRepository = idealDataRepository;
            _MetricService = MetricService;
            _TargetService = targetService;
        }

        public async Task<HealthLogOutputDTO> AddHealthLog(HealthLogInputDTO healthLogInputDTO, int UserId)
        {
            try
            {
                var healthlogs = await _HealthLogRepository.GetAll();
                var filteredHealthLogs = healthlogs
                    .Where(log => log.PreferenceId == healthLogInputDTO.PreferenceId && log.Created_at.Date == DateTime.Now.Date)
                    .ToList();
                if (filteredHealthLogs.Count != 0)
                    throw new EntityAlreadyExistsException("Health Log already entered!");
                else throw new NoItemsFoundException();
            }
            catch(NoItemsFoundException)
            {
                try
                {
                    HealthLog healthLog = new HealthLog();
                    healthLog.PreferenceId = healthLogInputDTO.PreferenceId;
                    healthLog.value = healthLogInputDTO.value;
                    healthLog.Created_at = DateTime.Now;
                    healthLog.Updated_at = DateTime.Now;
                    healthLog.HealthStatus = await calculateHealthStatus(healthLogInputDTO, UserId);
                    await _HealthLogRepository.Add(healthLog);

                    HealthLogOutputDTO healthLogOutputDTO = new HealthLogOutputDTO();
                    healthLogOutputDTO.HealthLogId = healthLog.Id;
                    healthLogOutputDTO.HealthStatus = healthLog.HealthStatus.ToString();
                    healthLogOutputDTO.TargetStatus = await calculateTargetStatus(healthLogInputDTO, UserId);
                    return healthLogOutputDTO;
                }
                catch { throw; }
            }
        }

        private async Task<string> calculateTargetStatus(HealthLogInputDTO healthLogInputDTO, int UserId)
        {
            var target = await _TargetService.GetTodaysTarget(healthLogInputDTO.PreferenceId, UserId);
            if(healthLogInputDTO.value >= target.TargetMinValue && healthLogInputDTO.value <= target.TargetMaxValue)
            {
                Target TargetToUpdate = await _TargetService.GetTargetById(target.Id);
                TargetToUpdate.TargetStatus = TargetStatusEnum.TargetStatus.Achieved;
                TargetToUpdate.Updated_at = DateTime.Now;
                await _TargetService.UpdateTargetRepo(TargetToUpdate);

                return TargetStatusEnum.TargetStatus.Achieved.ToString();
            }
            return TargetStatusEnum.TargetStatus.Not_Achieved.ToString();
        }

        private async Task<HealthStatusEnum.HealthStatus> calculateHealthStatus(HealthLogInputDTO healthlog, int UserId)
        {
            try
            {
                var IdealVals = await _IdealDataRepository.GetAll();
                int MetricId = await _MetricService.GetMetricIdFromPreferenceId(healthlog.PreferenceId);
                var Metric = await _MetricService.GetMetricById(MetricId);

                // There is no Ideal Height
                if (Metric.MetricType == "Height")
                    return HealthStatusEnum.HealthStatus.NoStatus;

                // For Weight, BMI is calculated
                if(Metric.MetricType == "Weight")
                {
                    var healthlogs = await _HealthLogRepository.GetAll();
                    int prefId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId("Height", UserId);
                    var heightlogs = healthlogs.Where(log => log.PreferenceId == prefId).OrderBy(x => x.Created_at).ToList();
                    if (healthlogs.Count == 0)
                        throw new EntityNotFoundException("Height Log not entered");
                    var heightlog = heightlogs[healthlogs.Count - 1];

                    var BMI = (healthlog.value / (heightlog.value * heightlog.value));
                    healthlog.value = BMI;

                    var BMIMetric = await _MetricService.FindMetricByMetricType("BMI");
                    MetricId = BMIMetric.Id;
                }

                // Health Status is calculated
                var MetricVals = IdealVals.Where(val =>  val.MetricId == MetricId).ToList();
                foreach (var val in MetricVals)
                {
                    if(healthlog.value >= val.MinVal && healthlog.value <= val.MaxVal)
                    {
                        return val.HealthStatus;
                    }
                }
                throw new NoItemsFoundException("No Ideal Value data found!");
            }
            catch { throw; }

        }
    }
}
