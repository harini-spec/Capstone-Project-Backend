using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.MetricPreference;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using System;
using System.Collections.Generic;

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

        public async Task<PreferenceOutputDTO> GetPreferenceDTOByPrefId(int PrefId)
        {
            try
            {
                var MetricId = await GetMetricIdFromPreferenceId(PrefId);
                var Metric = await GetMetricById(MetricId);

                PreferenceOutputDTO preferenceOutputDTO = new PreferenceOutputDTO();
                preferenceOutputDTO.PreferenceId = PrefId;
                preferenceOutputDTO.MetricType = Metric.MetricType;
                preferenceOutputDTO.MetricUnit = Metric.MetricUnit;
                return preferenceOutputDTO;
            }
            catch { throw; }
        }


        public async Task<string> AddPreference(List<string> Preferences, int UserId, string Role)
        {
            try
            {
                User user = await _UserService.GetUserById(UserId);

                List<string> ExistingPrefs = new List<string>();
                try
                {
                    var UserPrefs = await GetPreferencesListOfUser(UserId, Role);
                    foreach (var pref in UserPrefs)
                    {
                        ExistingPrefs.Add(pref.MetricType);
                    }
                }
                catch { }


                bool found = false;
                if (Role == "User")
                {
                    if(!Preferences.Contains("Height"))
                        Preferences.Add("Height");
                    if(!Preferences.Contains("Weight"))
                        Preferences.Add("Weight");

                    foreach(var pref in Preferences)
                    {
                        if (pref == "Height" && ExistingPrefs.Contains(pref))
                            continue;
                        else if(pref == "Weight" && ExistingPrefs.Contains(pref))
                            continue;

                        if (ExistingPrefs.Contains(pref))
                        {
                            found = true;
                        } 
                        else
                        {
                            UserPreference userPref = await MapUserPreferenceStringToUserPreference(pref, UserId);
                            await _UserPreferenceRepository.Add(userPref);
                            user.is_preferenceSet = true;
                            await _UserService.UpdateUser(user);
                        }
                    }
                }
                else
                {
                    foreach (var pref in Preferences)
                    {
                        if (ExistingPrefs.Contains(pref))
                        {
                            found = true;
                        }
                        else
                        {
                            MonitorPreference monitorPref = await MapMonitorPreferenceStringToMonitorPreference(pref, UserId);
                            await _MonitorPreferenceRepository.Add(monitorPref);
                            user.is_preferenceSet = true;
                            await _UserService.UpdateUser(user);
                        }
                    }
                }
                if (found)
                    throw new EntityAlreadyExistsException("Some monitor Preferences already exist. Choose again!");
                else
                    return "Successfully added";
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

        public async Task<List<string>> GetAllNotSelectedPrefs(int UserId, string Role)
        {
            try
            {
                var metrics = await _MetricRepository.GetAll();
                List<string> result = new List<string>();
                List<string> ExistingPrefs = new List<string>();

                try
                {
                    var UserPrefs = await GetPreferencesListOfUser(UserId, Role);
                    foreach (var pref in UserPrefs)
                    {
                        ExistingPrefs.Add(pref.MetricType);
                    }
                }
                catch { }

                foreach (var metric in metrics)
                {
                    if (metric.MetricType == "Height" || metric.MetricType == "Weight" || ExistingPrefs.Contains(metric.MetricType))
                        continue;
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

        public async Task<List<PreferenceOutputDTO>> GetPreferencesListOfUser(int UserId, string Role)
        {
            try
            {
                List<PreferenceOutputDTO> result = new List<PreferenceOutputDTO>();
                if (Role == "User")
                {
                    var prefs = await GetAllPreferencesOfUser(UserId);

                    foreach (var pref in prefs)
                    {
                        PreferenceOutputDTO preferenceOutputDTO = new PreferenceOutputDTO();
                        var metric = await _MetricRepository.GetById(pref.MetricId);

                        preferenceOutputDTO.PreferenceId = pref.Id;
                        preferenceOutputDTO.MetricType = metric.MetricType.ToString();
                        preferenceOutputDTO.MetricUnit = metric.MetricUnit;
                        result.Add(preferenceOutputDTO);
                    }
                    return result;
                }
                else
                {
                    var prefs = await GetAllMonitorPreferencesOfCoach(UserId);

                    foreach (var pref in prefs)
                    {
                        PreferenceOutputDTO preferenceOutputDTO = new PreferenceOutputDTO();
                        var metric = await _MetricRepository.GetById(pref.MetricId);

                        preferenceOutputDTO.PreferenceId = pref.Id;
                        preferenceOutputDTO.MetricType = metric.MetricType.ToString();
                        preferenceOutputDTO.MetricUnit = metric.MetricUnit;
                        result.Add(preferenceOutputDTO);
                    }
                    return result;
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<int> GetMetricIdFromPreferenceId(int PrefId)
        {
            try
            {
                var pref = await _UserPreferenceRepository.GetById(PrefId);
                return pref.MetricId;
            }
            catch { throw; }
        }

        public async Task<int> FindPreferenceIdFromMetricTypeAndUserId(string MetricType, int UserId)
        {
            try
            {
                var metric = await FindMetricByMetricType(MetricType);
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

        #region Mappers

        private async Task<UserPreference> MapUserPreferenceStringToUserPreference(string preference, int UserId)
        {
                UserPreference userPref = new UserPreference();
                userPref.UserId = UserId;
                userPref.Created_at = DateTime.Now;
                userPref.Updated_at = DateTime.Now;

                var metric = await FindMetricByMetricType(preference);
                userPref.MetricId = metric.Id;

                return userPref;
        }

        private async Task<MonitorPreference> MapMonitorPreferenceStringToMonitorPreference(string preference, int userId)
        {
                MonitorPreference monitorPref = new MonitorPreference();
                monitorPref.CoachId = userId;
                monitorPref.Created_at = DateTime.Now;
                monitorPref.Updated_at = DateTime.Now;

                var metric = await FindMetricByMetricType(preference);
                monitorPref.MetricId = metric.Id;

                return monitorPref;
        }

        #endregion
    }
}
