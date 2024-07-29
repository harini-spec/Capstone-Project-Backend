using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Graph;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using System.Collections.Generic;

namespace HealthTracker.Services.Classes
{
    public class GraphService : IGraphService
    {
        private readonly IRepository<int, HealthLog> _HealthLogRepository;
        private readonly IRepository<int, IdealData> _IdealDataRepository;
        private readonly IMetricService _MetricService;

        public GraphService(IRepository<int, HealthLog> healthLogRepository, IMetricService metricService, IRepository<int, IdealData> idealDataRepository)
        {
            _HealthLogRepository = healthLogRepository;
            _MetricService = metricService;
            _IdealDataRepository = idealDataRepository;
        }

        public DateTime GetFirstDayOfCurrentWeekStartingSunday()
        {
            DateTime currentDate = DateTime.Now;
            int daysToSubtract = (int)currentDate.DayOfWeek;

            DateTime firstDayOfWeek = currentDate.AddDays(-daysToSubtract);
            return firstDayOfWeek.Date;
        }

        public DateTime GetPreviousSunday()
        {
            DateTime currentDate = DateTime.Now;
            int daysToSubtract = (int)currentDate.DayOfWeek + 7; // Sunday is 0, so add 1 to get the previous Sunday

            DateTime previousSunday = currentDate.AddDays(-daysToSubtract);
            return previousSunday.Date;
        }


        // Duration - This Week, Last Week, This Month, Last Month, Overall
        public async Task<List<GraphDataOutputDTO>> GetGraphData(string MetricType, string Duration, int UserId)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime Start_Date = DateTime.Now;
                var HealthLogs = await _HealthLogRepository.GetAll();
                var PreferenceId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId(MetricType, UserId);
                List<HealthLog> ResultLogs = new List<HealthLog>();

                if (Duration == "This Week")
                {
                    Start_Date = GetFirstDayOfCurrentWeekStartingSunday();
                    ResultLogs = HealthLogs.Where(log => log.Created_at >= Start_Date && PreferenceId == log.PreferenceId).ToList();
                }
                else if (Duration == "Last Week")
                {
                    Start_Date = GetPreviousSunday();
                    DateTime End_Date = GetFirstDayOfCurrentWeekStartingSunday();
                    ResultLogs = HealthLogs.Where(log => log.Created_at >= Start_Date && log.Created_at < End_Date && PreferenceId == log.PreferenceId).ToList();
                }
                else if (Duration == "This Month")
                {
                    Start_Date = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ResultLogs = HealthLogs.Where(log => log.Created_at >= Start_Date && PreferenceId == log.PreferenceId).ToList();
                }
                else if (Duration == "Last Month")
                {
                    Start_Date = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
                    DateTime End_Date = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ResultLogs = HealthLogs.Where(log => log.Created_at >= Start_Date && log.Created_at < End_Date && PreferenceId == log.PreferenceId).ToList();
                }
                else
                {
                    ResultLogs = HealthLogs.Where(log => PreferenceId == log.PreferenceId).ToList();
                }
                if (ResultLogs.Count == 0)
                    throw new NoItemsFoundException("No Log Records found!");
                return MapHealthLogsToGraphDataOutputDTOs(ResultLogs, UserId);
            }
            catch { throw; }
        }

        public async Task<GraphDataRangeOutputDTO> GetGraphDataHealthyRange(string MetricType, int UserId)
        {
            try
            {
                var Metric = await _MetricService.FindMetricByMetricType(MetricType);
                var IdealData = await _IdealDataRepository.GetAll();
                var MetricIdealData = IdealData
                    .Where(idealData => idealData.MetricId == Metric.Id &&
                                        (idealData.HealthStatus == Models.ENUMs.HealthStatusEnum.HealthStatus.Excellent ||
                                         idealData.HealthStatus == Models.ENUMs.HealthStatusEnum.HealthStatus.Fair))
                    .ToList();

                if (MetricIdealData.Count == 0)
                    throw new NoItemsFoundException("No Ideal Data values found!");

                GraphDataRangeOutputDTO graphDataOutputDTO = new GraphDataRangeOutputDTO
                {
                    MetricType = MetricType,
                    MetricUnit = Metric.MetricUnit
                };

                var val_range = new List<float>();
                foreach (var data in MetricIdealData)
                {
                    val_range.Add(data.MinVal);
                    val_range.Add(data.MaxVal);
                }

                val_range = val_range.OrderBy(a => a).ToList();
                graphDataOutputDTO.MinValue = val_range.First();
                graphDataOutputDTO.MaxValue = val_range.Last();

                return graphDataOutputDTO;
            }
            catch
            {
                throw;
            }

        }


        #region Mappers

        private List<GraphDataOutputDTO> MapHealthLogsToGraphDataOutputDTOs(List<HealthLog> healthLogs, int UserId)
        {
            List<GraphDataOutputDTO> result = new List<GraphDataOutputDTO>(); 
            foreach (var healthLog in healthLogs)
            {
                result.Add(MapHealthLogToGraphDataOutputDTO(healthLog, UserId));
            }
            return result;
        }

        private GraphDataOutputDTO MapHealthLogToGraphDataOutputDTO(HealthLog healthLog, int UserId)
        {
            GraphDataOutputDTO graphDataOutputDTO = new GraphDataOutputDTO();
            graphDataOutputDTO.LogId = healthLog.Id;
            graphDataOutputDTO.LogDate = healthLog.Created_at;
            graphDataOutputDTO.Value = healthLog.value;
            graphDataOutputDTO.HealthStatus = healthLog.HealthStatus.ToString();
            graphDataOutputDTO.UserId = UserId;
            return graphDataOutputDTO;
        }

        #endregion
    }
}
