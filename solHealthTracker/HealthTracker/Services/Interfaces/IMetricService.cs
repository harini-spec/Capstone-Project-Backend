namespace HealthTracker.Services.Interfaces
{
    public interface IMetricService
    {
        public Task<List<string>> GetAllMetrics();

        public Task<string> AddUserPreference(List<string> UserPreferences);
        public Task<string> AddCoachMonitorPreference(List<string> UserPreferences);

        public Task<List<string>> GetUserPreferences(int UserId);
        public Task<List<string>> GetCoachMonitorPreferences(int UserId);

    }
}
