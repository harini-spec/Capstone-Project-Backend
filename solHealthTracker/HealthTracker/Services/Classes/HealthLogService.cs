using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using System.Collections.Immutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<AddHealthLogOutputDTO> AddHealthLog(AddHealthLogInputDTO healthLogInputDTO, int UserId)
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
            catch (NoItemsFoundException)
            {
                HealthLog healthLog = new HealthLog();
                healthLog.PreferenceId = healthLogInputDTO.PreferenceId;
                healthLog.value = healthLogInputDTO.value;
                healthLog.Created_at = DateTime.Now;
                healthLog.Updated_at = DateTime.Now;
                healthLog.HealthStatus = await calculateHealthStatus(healthLogInputDTO, UserId);
                await _HealthLogRepository.Add(healthLog);

                AddHealthLogOutputDTO healthLogOutputDTO = new AddHealthLogOutputDTO();
                healthLogOutputDTO.HealthLogId = healthLog.Id;
                healthLogOutputDTO.HealthStatus = healthLog.HealthStatus.ToString();
                healthLogOutputDTO.TargetStatus = await calculateTargetStatus(healthLogInputDTO, UserId);
                return healthLogOutputDTO;
            }
        }

        public async Task<GetHealthLogOutputDTO> GetHealthLog(int PrefId, int UserId)
        {
            try
            {
                var healthlogs = await _HealthLogRepository.GetAll();
                var healthlog = healthlogs.Where(log => log.PreferenceId == PrefId && log.Created_at.Date == DateTime.Now.Date).ToList();
                if (healthlog.Count > 0)
                {
                    return await MapHealthLogToGetHealthLogOutputDTO(healthlog[0], UserId);
                }
                else throw new NoItemsFoundException("No Health logs found!");
            }
            catch { throw; }
        }

        public async Task<AddHealthLogOutputDTO> UpdateHealthLog(int HealthLogId, float value, int UserId)
        {
            HealthLog log = null;
            try
            {
                log = await _HealthLogRepository.GetById(HealthLogId);
                log.value = value;
                log.Updated_at = DateTime.Now;

                AddHealthLogInputDTO healthLogInput = new AddHealthLogInputDTO();
                healthLogInput.PreferenceId = log.PreferenceId;
                healthLogInput.value = value;
                log.HealthStatus = await calculateHealthStatus(healthLogInput, UserId);
                await _HealthLogRepository.Update(log);

                AddHealthLogOutputDTO healthLogOutputDTO = new AddHealthLogOutputDTO();
                healthLogOutputDTO.HealthLogId = log.Id;
                healthLogOutputDTO.HealthStatus = log.HealthStatus.ToString();
                healthLogOutputDTO.TargetStatus = await calculateTargetStatus(healthLogInput, UserId);
                return healthLogOutputDTO;
            }
            catch
            {
                throw;
            }
        }

        private async Task<GetHealthLogOutputDTO> MapHealthLogToGetHealthLogOutputDTO(HealthLog healthlog, int UserId)
        {
            GetHealthLogOutputDTO getHealthLogOutputDTO = new GetHealthLogOutputDTO();
            getHealthLogOutputDTO.Id = healthlog.Id;
            getHealthLogOutputDTO.PreferenceId = healthlog.PreferenceId;
            getHealthLogOutputDTO.value = healthlog.value;
            getHealthLogOutputDTO.HealthStatus = healthlog.HealthStatus.ToString();

            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO();
            addHealthLogInputDTO.PreferenceId = healthlog.PreferenceId;
            addHealthLogInputDTO.value = healthlog.value;
            getHealthLogOutputDTO.TargetStatus = await calculateTargetStatus(addHealthLogInputDTO, UserId);
            getHealthLogOutputDTO.Created_at = healthlog.Created_at;
            getHealthLogOutputDTO.Updated_at = healthlog.Updated_at;
            return getHealthLogOutputDTO;
        }

        private async Task<string> calculateTargetStatus(AddHealthLogInputDTO healthLogInputDTO, int UserId)
        {
            try
            {
                var target = await _TargetService.GetTodaysTarget(healthLogInputDTO.PreferenceId, UserId);

                Target TargetToUpdate = await _TargetService.GetTargetById(target.Id);
                TargetToUpdate.Updated_at = DateTime.Now;

                if (healthLogInputDTO.value >= target.TargetMinValue && healthLogInputDTO.value <= target.TargetMaxValue)
                {
                    TargetToUpdate.TargetStatus = TargetStatusEnum.TargetStatus.Achieved;
                    await _TargetService.UpdateTargetRepo(TargetToUpdate);
                    return TargetStatusEnum.TargetStatus.Achieved.ToString();
                }
                else
                {
                    TargetToUpdate.TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved;
                    await _TargetService.UpdateTargetRepo(TargetToUpdate);
                    return TargetStatusEnum.TargetStatus.Not_Achieved.ToString();
                }
            }
            catch(NoItemsFoundException) 
            { 
                return null;
            }
        }

        private async Task<HealthStatusEnum.HealthStatus> calculateHealthStatus(AddHealthLogInputDTO healthlog, int UserId)
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
