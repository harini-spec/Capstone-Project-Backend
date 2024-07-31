using HealthTracker.Models.DTOs.Suggestions;

namespace HealthTracker.Services.Interfaces
{
    public interface IProblemService
    {
        public Task<List<ProblemOutputDTO>> GetUserIdsWithProblems(int CoachId);
        public Task<string> AddSuggestion(SuggestionInputDTO suggestionInputDTO, int CoachId);
        public Task<List<SuggestionOutputDTO>> GetUserSuggestions(int UserId);
        public Task<List<SuggestionOutputDTO>> GetCoachSuggestionsForUser(int UserId, int CoachId);
        public Task<ProblemOutputDTO> GetProblemsOfUserId(int UserId);
    }
}
