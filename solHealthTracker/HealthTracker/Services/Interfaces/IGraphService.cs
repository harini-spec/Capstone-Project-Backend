using HealthTracker.Models.DTOs.Graph;

namespace HealthTracker.Services.Interfaces
{
    public interface IGraphService
    {
        public Task<List<GraphDataOutputDTO>> GetGraphData(string MetricType, string Duration, int UserId);

        public Task<GraphDataRangeOutputDTO> GetGraphDataHealthyRange(string MetricType, int UserId);
        public DateTime GetPreviousSunday();
        public DateTime GetFirstDayOfCurrentWeekStartingSunday();

    }
}
