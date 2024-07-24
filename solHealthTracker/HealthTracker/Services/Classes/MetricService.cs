using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class MetricService : IMetricService
    {
        private readonly IRepository<int, UserPreference> _UserPreferenceRepository;
        private readonly IRepository<int, MonitorPreference> _MonitorPreferenceRepository;
        private readonly IRepository<int, Metric> _MetricRepository;

        public MetricService(IRepository<int, UserPreference> UserPreferenceRepository, IRepository<int, MonitorPreference> MonitorPreferenceRepository, IRepository<int, Metric> metricRepository)
        {
            _MonitorPreferenceRepository = MonitorPreferenceRepository;
            _UserPreferenceRepository = UserPreferenceRepository;
            _MetricRepository = metricRepository;
        }

        public Task<string> AddCoachMonitorPreference(List<string> UserPreferences)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddUserPreference(List<string> UserPreferences)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllMetrics()
        {
            try
            {
                var metrics = await _MetricRepository.GetAll();
                List<string> result = new List<string>();
                foreach (var metric in metrics)
                {
                    result.Add(metric.MetricType);
                }
                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public Task<List<string>> GetCoachMonitorPreferences(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetUserPreferences(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
