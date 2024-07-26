﻿using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs.Target;

namespace HealthTracker.Services.Interfaces
{
    public interface ITargetService
    {
        public Task<string> AddTarget(TargetInputDTO targetInputDTO, int UserId);
        public Task<TargetOutputDTO> GetTodaysTarget(int PrefId, int UserId);
        public Task UpdateTargetRepo(Target target);
        public Task<string> UpdateTarget(UpdateTargetInputDTO updateTargetInputDTO, int UserId);
        public Task<Target> GetTargetById(int TargetId);
        public Task<string> calculateTargetStatus(AddHealthLogInputDTO healthLogInputDTO, int UserId);

    }
}
