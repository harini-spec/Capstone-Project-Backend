using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class TargetService : ITargetService
    {
        private readonly IRepository<int, Target> _TargetRepository;
        private readonly IMetricService _MetricService;

        public TargetService(IRepository<int, Target> TargetRepository, IMetricService metricService)
        {
            _TargetRepository = TargetRepository;
            _MetricService = metricService;
        }

        public async Task<bool> IsAddTargetPossible(TargetInputDTO targetInputDTO, int TargetId)
        {
            try
            {
                if (targetInputDTO.TargetDate.Date < DateTime.Now.Date)
                    throw new InvalidActionException("Can't create targets in the past");
            }
            catch { throw; }

            try
            {
                var targets = await _TargetRepository.GetAll();
                var found = false;
                foreach (var target in targets)
                {
                    if (target.TargetDate.Date == targetInputDTO.TargetDate.Date && TargetId != target.Id)
                    {
                        UserPreference pref = await _MetricService.FindUserPreferenceByPreferenceId(target.PreferenceId);
                        Metric metric = await _MetricService.GetMetricById(pref.MetricId);
                        if (target.PreferenceId == targetInputDTO.PreferenceId)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                return !found;
            }
            catch (NoItemsFoundException)
            {
                return true;
            }
            catch { throw; }
        }

        public async Task<string> AddTarget(TargetInputDTO targetInputDTO)
        {
            try
            {
                if (await IsAddTargetPossible(targetInputDTO, -1))
                {
                    await _TargetRepository.Add(MapTargetInputDTOToTarget(targetInputDTO));
                    return "Successfully added!";
                }
                else throw new EntityAlreadyExistsException("Target already exists for this date!");

            }
            catch (Exception)
            { throw; }
        }

        public async Task<TargetOutputDTO> GetTodaysTarget(int prefId, int UserId)
        {
            try
            {
                DateTime current_date = DateTime.Now;
                var targets = await _TargetRepository.GetAll();

                var filteredAndSortedTargets = targets
                .Where(x => x.TargetDate.Date >= current_date.Date && x.PreferenceId == prefId)
                .OrderBy(x => x.TargetDate)
                .ToList();

                if(filteredAndSortedTargets.Count == 0)
                    throw new NoItemsFoundException();
                return MapTargetToTargetOutputDTO(filteredAndSortedTargets[0]);
            }
            catch { throw; }
        }

        public async Task<string> calculateTargetStatus(AddHealthLogInputDTO healthLogInputDTO, int UserId)
        {
            try
            {
                var target = await GetTodaysTarget(healthLogInputDTO.PreferenceId, UserId);

                Target TargetToUpdate = await GetTargetById(target.Id);
                TargetToUpdate.Updated_at = DateTime.Now;

                if (healthLogInputDTO.value >= target.TargetMinValue && healthLogInputDTO.value <= target.TargetMaxValue)
                {
                    TargetToUpdate.TargetStatus = TargetStatusEnum.TargetStatus.Achieved;
                    await UpdateTargetRepo(TargetToUpdate);
                    return TargetStatusEnum.TargetStatus.Achieved.ToString();
                }
                else
                {
                    TargetToUpdate.TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved;
                    await UpdateTargetRepo(TargetToUpdate);
                    return TargetStatusEnum.TargetStatus.Not_Achieved.ToString();
                }
            }
            catch (NoItemsFoundException)
            {
                return null;
            }
        }

        public async Task<string> UpdateTarget(UpdateTargetInputDTO updateTargetInputDTO)
        {
            try
            {
                TargetInputDTO targetInputDTO = MapUpdateTargetDTOToTargetInputDTO(updateTargetInputDTO);

                if (await IsAddTargetPossible(targetInputDTO, updateTargetInputDTO.TargetId))
                {
                    var TargetToUpdate = await _TargetRepository.GetById(updateTargetInputDTO.TargetId);
                    TargetToUpdate.TargetMinValue = updateTargetInputDTO.TargetMinValue;
                    TargetToUpdate.TargetMaxValue = updateTargetInputDTO.TargetMaxValue;
                    TargetToUpdate.TargetDate = updateTargetInputDTO.TargetDate;
                    TargetToUpdate.TargetStatus = Models.ENUMs.TargetStatusEnum.TargetStatus.Not_Achieved;
                    TargetToUpdate.Updated_at = DateTime.Now;
                    await UpdateTargetRepo(TargetToUpdate);
                    return "Successfully updated!";
                }
                else throw new EntityAlreadyExistsException("Target already exists for this date!");
            }
            catch { throw; }
        }

        public async Task<Target> GetTargetById(int TargetId)
        {
            try
            {
                return await _TargetRepository.GetById(TargetId);
            }
            catch { throw; }
        }

        public async Task UpdateTargetRepo(Target target)
        {
            try
            {
                await _TargetRepository.Update(target);
            }
            catch { throw; }
        }


        #region Mappers

        private TargetInputDTO MapUpdateTargetDTOToTargetInputDTO(UpdateTargetInputDTO updateTargetInputDTO)
        {
            TargetInputDTO targetInputDTO = new TargetInputDTO();
            targetInputDTO.PreferenceId = updateTargetInputDTO.PreferenceId;
            targetInputDTO.TargetMaxValue = updateTargetInputDTO.TargetMaxValue;
            targetInputDTO.TargetMinValue = updateTargetInputDTO.TargetMinValue;
            targetInputDTO.TargetDate = updateTargetInputDTO.TargetDate;
            return targetInputDTO;
        }

        private TargetOutputDTO MapTargetToTargetOutputDTO(Target target)
        {
            TargetOutputDTO targetOutputDTO = new TargetOutputDTO();
            targetOutputDTO.Id = target.Id;
            targetOutputDTO.PreferenceId = target.PreferenceId;
            targetOutputDTO.TargetStatus = target.TargetStatus.ToString();
            targetOutputDTO.TargetMinValue = target.TargetMinValue;
            targetOutputDTO.TargetMaxValue = target.TargetMaxValue;
            targetOutputDTO.TargetDate = target.TargetDate;
            return targetOutputDTO;
        }

        private Target MapTargetInputDTOToTarget(TargetInputDTO targetInputDTO)
        {
            Target target = new Target();
            target.PreferenceId = targetInputDTO.PreferenceId;
            target.Created_at = DateTime.Now;
            target.Updated_at = DateTime.Now;
            target.TargetMaxValue = targetInputDTO.TargetMaxValue;
            target.TargetMinValue = targetInputDTO.TargetMinValue;
            target.TargetDate = targetInputDTO.TargetDate;
            target.TargetStatus = Models.ENUMs.TargetStatusEnum.TargetStatus.Not_Achieved;
            return target;
        }
        #endregion
    }
}
