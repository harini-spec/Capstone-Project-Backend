namespace HealthTracker.Services.Interfaces
{
    public interface IOpenAIService
    {
        public Task<string> GetCompletionAsync(string prompt);
    }
}
