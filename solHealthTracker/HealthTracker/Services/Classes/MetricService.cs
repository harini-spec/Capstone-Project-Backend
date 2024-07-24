using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class MetricService : IMetricService
    {
        private readonly IRepository<int, UserPreference> _UserPreferenceRepository;
        private readonly IRepository<int, MonitorPreference> _MonitorPreferenceRepository;
        private readonly IRepository<int, Metric> _MetricRepository;
        private readonly IUserService _UserService;

        public MetricService(IRepository<int, UserPreference> UserPreferenceRepository, IRepository<int, MonitorPreference> MonitorPreferenceRepository, IRepository<int, Metric> metricRepository, IUserService UserService)
        {
            _MonitorPreferenceRepository = MonitorPreferenceRepository;
            _UserPreferenceRepository = UserPreferenceRepository;
            _MetricRepository = metricRepository;
            _UserService = UserService;
        }

        public async Task<int> FindPreferenceIdFromMetricTypeAndUserId(string Metric_Type, int UserId)
        {
            try
            {
                var metric = await FindMetricByMetricType(Metric_Type);
                var prefs = await _UserPreferenceRepository.GetAll();
                var user_pref = prefs.ToList().SingleOrDefault(x => x.MetricId == metric.Id && x.UserId == UserId);
                if (user_pref == null)
                    throw new EntityNotFoundException("User Preference not found");
                return user_pref.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> AddPreference(List<string> Preferences, int UserId, string Role)
        {
            try
            {
                User user = await _UserService.GetUserById(UserId);
                if (user == null)
                    throw new UnauthorizedUserException("You are not logged in!");

                if (Role == "User")
                {
                    foreach (var preference in Preferences)
                    {
                        UserPreference userPref = await MapUserPreferenceStringToUserPreference(preference, UserId);
                        await _UserPreferenceRepository.Add(userPref);
                    }
                    user.is_preferenceSet = true;
                    await _UserService.UpdateUser(user);
                    return "Successfully added";
                }
                else
                {
                    foreach (var preference in Preferences)
                    {
                        MonitorPreference monitorPref = await MapMonitorPreferenceStringToMonitorPreference(preference, UserId);
                        await _MonitorPreferenceRepository.Add(monitorPref);
                    }
                    user.is_preferenceSet = true;
                    await _UserService.UpdateUser(user);
                    return "Successfully added";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Metric> FindMetricByMetricType(string MetricType)
        {
            try
            {
                var metrics = await _MetricRepository.GetAll();
                var metric = metrics.ToList().SingleOrDefault(x => x.MetricType.ToString() == MetricType);
                if (metric == null)
                    throw new EntityNotFoundException();
                return metric;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<UserPreference> FindUserPreferenceByPreferenceId(int prefId)
        {
            try
            {
                return await _UserPreferenceRepository.GetById(prefId);
            }
            catch { throw; }
        }

        public async Task<Metric> GetMetricById(int MetricId)
        {
            try
            {
                return await _MetricRepository.GetById(MetricId);
            }
            catch { throw; }
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

        public async Task<List<UserPreference>> GetAllPreferencesOfUser(int UserId)
        {
            try
            {
                var preferences = await _UserPreferenceRepository.GetAll();
                var userPref = preferences.ToList().FindAll(x => x.UserId == UserId);
                if (userPref.Count == 0)
                    throw new NoItemsFoundException();
                return userPref;    
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<MonitorPreference>> GetAllMonitorPreferencesOfCoach(int UserId)
        {
            try
            {
                var preferences = await _MonitorPreferenceRepository.GetAll();
                var monitorPref = preferences.ToList().FindAll(x => x.CoachId == UserId);
                if (monitorPref.Count == 0)
                    throw new NoItemsFoundException();
                return monitorPref;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> GetPreferencesListOfUser(int UserId, string Role)
        {
            try
            {
                if (Role == "User")
                {
                    var prefs = await GetAllPreferencesOfUser(UserId);
                    List<string> result = new List<string>();

                    foreach (var pref in prefs)
                    {
                        var metric = await _MetricRepository.GetById(pref.MetricId);
                        result.Add(metric.MetricType.ToString());
                    }
                    return result;
                }
                else
                {
                    var prefs = await GetAllMonitorPreferencesOfCoach(UserId);
                    List<string> result = new List<string>();

                    foreach (var pref in prefs)
                    {
                        var metric = await _MetricRepository.GetById(pref.MetricId);
                        result.Add(metric.MetricType.ToString());
                    }
                    return result;
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }


        #region Mappers

        private async Task<UserPreference> MapUserPreferenceStringToUserPreference(string preference, int UserId)
        {
            try
            {
                UserPreference userPref = new UserPreference();
                userPref.UserId = UserId;
                userPref.Created_at = DateTime.Now;
                userPref.Updated_at = DateTime.Now;

                var metric = await FindMetricByMetricType(preference);
                userPref.MetricId = metric.Id;

                return userPref;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<MonitorPreference> MapMonitorPreferenceStringToMonitorPreference(string preference, int userId)
        {
            try
            {
                MonitorPreference monitorPref = new MonitorPreference();
                monitorPref.CoachId = userId;
                monitorPref.Created_at = DateTime.Now;
                monitorPref.Updated_at = DateTime.Now;

                var metric = await FindMetricByMetricType(preference);
                monitorPref.MetricId = metric.Id;

                return monitorPref;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
