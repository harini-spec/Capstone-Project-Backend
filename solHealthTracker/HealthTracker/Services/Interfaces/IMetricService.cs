using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.MetricPreference;

namespace HealthTracker.Services.Interfaces
{
    public interface IMetricService
    {
        public Task<string> AddPreference(List<string> Preferences, int UserId, string Role);
        public Task<Metric> FindMetricByMetricType(string MetricType);
        public Task<List<string>> GetAllMetrics();
        public Task<List<PreferenceOutputDTO>> GetPreferencesListOfUser(int UserId, string Role);
        public Task<List<MonitorPreference>> GetAllMonitorPreferencesOfCoach(int UserId);
        public Task<List<UserPreference>> GetAllPreferencesOfUser(int UserId);
        public Task<UserPreference> FindUserPreferenceByPreferenceId(int prefId);
        public Task<Metric> GetMetricById(int MetricId);
        public Task<int> GetMetricIdFromPreferenceId(int PrefId);
        public Task<int> FindPreferenceIdFromMetricTypeAndUserId(string MetricType, int UserId);
    }
}
