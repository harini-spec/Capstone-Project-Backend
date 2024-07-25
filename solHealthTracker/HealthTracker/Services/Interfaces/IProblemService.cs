using HealthTracker.Models.DTOs.Suggestions;

namespace HealthTracker.Services.Interfaces
{
    public interface IProblemService
    {
        public Task<List<ProblemOutputDTO>> GetUserIdsWithProblems(int CoachId);
    }
}
