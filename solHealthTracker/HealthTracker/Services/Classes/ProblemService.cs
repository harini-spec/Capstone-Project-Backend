using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Suggestions;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class ProblemService : IProblemService
    {
        private readonly IRepository<int, HealthLog> _HealthLogRepository;
        private readonly IRepository<int, MonitorPreference> _MonitorPreferenceRepository;
        private readonly IRepository<int, Suggestion> _SuggestionRepository;
        private readonly IMetricService _MetricService;
        private readonly IUserService _UserService;

        public ProblemService(IRepository<int, HealthLog> healthLogRepository, IRepository<int, MonitorPreference> monitorPreferenceRepository, IMetricService metricService, IRepository<int, Suggestion> suggestionRepository, IUserService userService)
        {
            _HealthLogRepository = healthLogRepository;
            _MonitorPreferenceRepository = monitorPreferenceRepository;
            _MetricService = metricService;
            _SuggestionRepository = suggestionRepository;
            _UserService = userService;
        }

        public async Task<string> AddSuggestion(SuggestionInputDTO suggestionInputDTO, int CoachId)
        {
            try
            {
                var User = await _UserService.GetUserById(suggestionInputDTO.UserId);
                if (User.Role.ToString() == "Coach")
                    throw new InvalidActionException("Can't add suggestions for Coach!");

                Suggestion SuggestionToBeAdded = new Suggestion();
                SuggestionToBeAdded.UserId = suggestionInputDTO.UserId;
                SuggestionToBeAdded.CoachId = CoachId;
                SuggestionToBeAdded.Description = suggestionInputDTO.Suggestion;
                SuggestionToBeAdded.Created_at = DateTime.Now;
                SuggestionToBeAdded.Updated_at = DateTime.Now;
                await _SuggestionRepository.Add(SuggestionToBeAdded);
                return "Successfully Added!";
            }
            catch { throw; }
        }

        public async Task<ProblemOutputDTO> GetProblemsOfUserId(int UserId)
        {
            try
            {
                var Logs = new List<HealthLog>();
                try
                {
                    Logs = await _HealthLogRepository.GetAll();
                }
                catch
                {
                    throw new NoItemsFoundException("No Problem Logs for today!");
                }

                // Filter logs for today's poor health status
                var TodaysPoorLogs = Logs
                    .Where(log => log.Created_at.Date == DateTime.Now.Date && log.HealthStatus == Models.ENUMs.HealthStatusEnum.HealthStatus.Poor)
                    .ToList();

                if (TodaysPoorLogs.Count == 0)
                    throw new NoItemsFoundException("No Problem Logs for today!");

                var UserPrefs = await _MetricService.GetPreferencesListOfUser(UserId, "User");

                // Filter logs based on user's preferences
                var filteredLogs = TodaysPoorLogs
                    .Where(log => UserPrefs.Any(pref => pref.PreferenceId == log.PreferenceId))
                    .ToList();

                if (filteredLogs.Count == 0)
                    throw new NoItemsFoundException("No Problem Logs for today!");

                return await MapLogsToProblemOutputDTO(filteredLogs, UserId);
            }
            catch { throw; }
        }

        public async Task<ProblemOutputDTO> MapLogsToProblemOutputDTO(List<HealthLog> Logs, int UserId)
        {
            try
            {
                ProblemOutputDTO problemOutputDTO = new ProblemOutputDTO();
                List<string> ProblemMetrics = new List<string>();
                foreach (var log in Logs)
                {
                    var Preference = await _MetricService.GetPreferenceDTOByPrefId(log.PreferenceId);
                    ProblemMetrics.Add(Preference.MetricType);
                }

                var User = await _UserService.GetUserById(UserId);
                problemOutputDTO.UserId = UserId;
                problemOutputDTO.UserName = User.Name;
                problemOutputDTO.Age = User.Age;
                problemOutputDTO.Gender = User.Gender.ToString();
                problemOutputDTO.MetricsWithProblem = ProblemMetrics;
                return problemOutputDTO;
            }
            catch { throw; }
        }

        public async Task<List<ProblemOutputDTO>> GetUserIdsWithProblems(int CoachId)
        {
            try
            {
                var AllMonitorPrefs = new List<MonitorPreference>();
                try
                {
                    AllMonitorPrefs = await _MonitorPreferenceRepository.GetAll();
                }
                catch { throw new NoItemsFoundException("No Monitor Preferences found!"); }

                var Suggestions = new List<Suggestion>();
                try
                {
                    Suggestions = await _SuggestionRepository.GetAll();
                }
                catch { }

                var CoachMonitorPrefs = AllMonitorPrefs.Where(pref => pref.CoachId == CoachId).ToList();
                if (CoachMonitorPrefs.Count == 0)
                    throw new NoItemsFoundException("Coach Preferences not set!");

                var Logs = new List<HealthLog>();
                try
                {
                    Logs = await _HealthLogRepository.GetAll();
                }
                catch { throw new NoItemsFoundException("No Problem Logs for today!");  }
                var TodaysPoorLogs = Logs.Where(log => log.Created_at.Date == DateTime.Now.Date && log.HealthStatus == Models.ENUMs.HealthStatusEnum.HealthStatus.Poor).ToList();
                if (TodaysPoorLogs.Count == 0)
                    throw new NoItemsFoundException("No Problem Logs for today!");

                var Result = new List<HealthLog>();
                foreach (var coach_pref in CoachMonitorPrefs)
                {
                    foreach (var log in TodaysPoorLogs)
                    {
                        var user_pref = await _MetricService.FindUserPreferenceByPreferenceId(log.PreferenceId);
                        if (user_pref.MetricId == coach_pref.MetricId)
                            Result.Add(log);
                    }
                }
                if(Result.Count == 0)
                    throw new NoItemsFoundException("No Problem Logs for today!");

                return await MapHealthLogsToDictionary(Result, CoachId);
            }
            catch { throw; }
        }

        public async Task<List<SuggestionOutputDTO>> GetUserSuggestions(int UserId)
        {
            try
            {
                var suggestions = await _SuggestionRepository.GetAll();
                var UserSuggestions = suggestions.Where(suggestion => suggestion.UserId == UserId).ToList();
                if (UserSuggestions.Count == 0)
                    throw new NoItemsFoundException("No Suggestions Found!");
                return await MapSuggestionsToSuggestionOutputDTOs(UserSuggestions);
            }
            catch { throw; }
        }

        public async Task<List<SuggestionOutputDTO>> GetCoachSuggestionsForUser(int UserId, int CoachId)
        {
            try
            {
                var suggestions = await _SuggestionRepository.GetAll();
                var UserSuggestions = suggestions.Where(suggestion => suggestion.UserId == UserId && suggestion.CoachId == CoachId).ToList();
                if (UserSuggestions.Count == 0)
                    throw new NoItemsFoundException("No Suggestions Found!");
                return await MapSuggestionsToSuggestionOutputDTOs(UserSuggestions);

            }
            catch { throw; }
        }


        #region Mappers

        private async Task<List<SuggestionOutputDTO>> MapSuggestionsToSuggestionOutputDTOs(List<Suggestion> userSuggestions)
        {

            List<SuggestionOutputDTO> suggestionOutputDTOs = new List<SuggestionOutputDTO>();
            foreach (var suggestion in userSuggestions)
            {
                suggestionOutputDTOs.Add(await MapSuggestionToSuggestionOutputDTO(suggestion));
            }
            return suggestionOutputDTOs;
        }

        private async Task<SuggestionOutputDTO> MapSuggestionToSuggestionOutputDTO(Suggestion userSuggestion)
        {
                SuggestionOutputDTO suggestionOutputDTO = new SuggestionOutputDTO();
                suggestionOutputDTO.Id = userSuggestion.Id;
                suggestionOutputDTO.UserId = userSuggestion.UserId;
                suggestionOutputDTO.CoachId = userSuggestion.CoachId;
                suggestionOutputDTO.Description = userSuggestion.Description;
                suggestionOutputDTO.Created_at = userSuggestion.Created_at;
                suggestionOutputDTO.Updated_at = userSuggestion.Updated_at;

                var Coach = await _UserService.GetUserById(userSuggestion.CoachId);
                suggestionOutputDTO.CoachName = Coach.Name;
                return suggestionOutputDTO;
        }

        private async Task<List<ProblemOutputDTO>> MapHealthLogsToDictionary(List<HealthLog> result, int CoachId)
        {
            try
            {
                List<ProblemOutputDTO> problemOutputDTOs = new List<ProblemOutputDTO>();
                Dictionary<int, List<string>> UserIdWithMetrics = new Dictionary<int, List<string>>();

                foreach (var log in result)
                {
                    var user_pref = await _MetricService.FindUserPreferenceByPreferenceId(log.PreferenceId);
                    if (user_pref != null)
                    {
                        if (!UserIdWithMetrics.ContainsKey(user_pref.UserId))
                        {
                            UserIdWithMetrics[user_pref.UserId] = new List<string>();
                        }

                        var Metric = await _MetricService.GetMetricById(user_pref.MetricId);
                        UserIdWithMetrics[user_pref.UserId].Add(Metric.MetricType);
                    }
                }
                return await MapDictionaryToProblemOutputDTOs(UserIdWithMetrics, CoachId);
            }
            catch { throw; }
        }

        private async Task<List<ProblemOutputDTO>> MapDictionaryToProblemOutputDTOs(Dictionary<int, List<string>> userIdWithMetrics, int CoachId)
        {
            // User ID
            var AllSuggestions = new List<Suggestion>();
            try
            {
                AllSuggestions = await _SuggestionRepository.GetAll();
            }
            catch { }

            if(AllSuggestions.Count != 0)
            {
                var CoachSuggestionsForToday = AllSuggestions.Where(suggestion => suggestion.CoachId == CoachId && suggestion.Created_at.Date == DateTime.Now.Date).ToList();
                if(CoachSuggestionsForToday.Count != 0)
                {
                    foreach(var suggestion in CoachSuggestionsForToday)
                    {
                        if (userIdWithMetrics.ContainsKey(suggestion.UserId))
                        {
                            userIdWithMetrics.Remove(suggestion.UserId);
                        }
                    }
                }
            }

            List<ProblemOutputDTO> result = new List<ProblemOutputDTO>();
            foreach (var Item in userIdWithMetrics)
            {
                ProblemOutputDTO problemOutputDTO = new ProblemOutputDTO();
                try
                {
                    var User = await _UserService.GetUserById(Item.Key);
                    problemOutputDTO.UserId = Item.Key;
                    problemOutputDTO.UserName = User.Name;
                    problemOutputDTO.Age = User.Age;
                    problemOutputDTO.Gender = User.Gender.ToString();
                }
                catch { throw; }
                problemOutputDTO.MetricsWithProblem = Item.Value;
                result.Add(problemOutputDTO);
            }
            return result;
        }

        #endregion

    }
}
