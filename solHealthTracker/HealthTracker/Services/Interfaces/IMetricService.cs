using HealthTracker.Models.DBModels;

namespace HealthTracker.Services.Interfaces
{
    public interface IMetricService
    {
        public Task<string> AddPreference(List<string> Preferences, int UserId, string Role);
        public Task<Metric> FindMetricByMetricType(string MetricType);
        public Task<List<string>> GetAllMetrics();
        public Task<List<string>> GetPreferencesListOfUser(int UserId, string Role);
        public Task<List<MonitorPreference>> GetAllMonitorPreferencesOfCoach(int UserId);
        public Task<List<UserPreference>> GetAllPreferencesOfUser(int UserId);
        public Task<int> FindPreferenceIdFromMetricTypeAndUserId(string Metric_Type, int UserId);

    }
}
