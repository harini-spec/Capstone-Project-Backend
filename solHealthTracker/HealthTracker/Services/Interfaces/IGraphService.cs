using HealthTracker.Models.DTOs.Graph;

namespace HealthTracker.Services.Interfaces
{
    public interface IGraphService
    {
        public Task<List<GraphDataOutputDTO>> GetGraphData(string MetricType, string Duration, int UserId);
    }
}
