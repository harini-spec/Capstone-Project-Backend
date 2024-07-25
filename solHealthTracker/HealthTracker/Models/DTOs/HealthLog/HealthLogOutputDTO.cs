﻿using HealthTracker.Models.ENUMs;

namespace HealthTracker.Models.DTOs.HealthLog
{
    public class HealthLogOutputDTO
    {
        public int HealthLogId { get; set; }
        public string HealthStatus { get; set; }
        public string TargetStatus { get; set;}

    }
}
