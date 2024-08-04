using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTracker.Models.DTOs.Coach
{
    public class GetCoachDataDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
    }
}
