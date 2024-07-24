using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Target;
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

        public async Task<bool> IsAddTargetPossible(TargetInputDTO targetInputDTO, int UserId, int TargetId)
        {
            if (targetInputDTO.TargetDate.Date < DateTime.Now.Date)
                throw new InvalidActionException("Can't create targets in the past");

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
                        if (metric.MetricType == targetInputDTO.MetricType && pref.UserId == UserId)
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

        public async Task<string> AddTarget(TargetInputDTO targetInputDTO, int UserId)
        {
            try
            {
                if (await IsAddTargetPossible(targetInputDTO, UserId, -1))
                {
                    int prefId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId(targetInputDTO.MetricType, UserId);
                    await _TargetRepository.Add(MapTargetInputDTOToTarget(targetInputDTO, prefId));
                    return "Successfully added!";
                }
                else throw new TargetAlreadyExistsException();

            }
            catch (Exception)
            { throw; }
        }

        public async Task<TargetOutputDTO> GetTodaysTarget(string MetricType, int UserId)
        {
            try
            {
                DateTime current_date = DateTime.Now;
                var targets = await _TargetRepository.GetAll();
                int prefId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId(MetricType, UserId);

                var filteredAndSortedTargets = targets
                .Where(x => x.TargetDate.Date >= current_date.Date && x.PreferenceId == prefId && x.TargetStatus == Models.ENUMs.TargetStatusEnum.TargetStatus.Not_Achieved)
                .OrderBy(x => x.TargetDate)
                .ToList();

                if(filteredAndSortedTargets.Count == 0)
                    throw new NoItemsFoundException();
                return MapTargetToTargetOutputDTO(filteredAndSortedTargets[0]);
            }
            catch { throw; }
        }

        public async Task<string> UpdateTarget(UpdateTargetInputDTO updateTargetInputDTO, int UserId)
        {
            try
            {
                TargetInputDTO targetInputDTO = MapUpdateTargetDTOToTargetInputDTO(updateTargetInputDTO);

                if (await IsAddTargetPossible(targetInputDTO, UserId, updateTargetInputDTO.TargetId))
                {
                    int prefId = await _MetricService.FindPreferenceIdFromMetricTypeAndUserId(targetInputDTO.MetricType, UserId);

                    var TargetToUpdate = await _TargetRepository.GetById(updateTargetInputDTO.TargetId);
                    TargetToUpdate.TargetMinValue = updateTargetInputDTO.TargetMinValue;
                    TargetToUpdate.TargetMaxValue = updateTargetInputDTO.TargetMaxValue;
                    TargetToUpdate.TargetDate = updateTargetInputDTO.TargetDate;
                    TargetToUpdate.Updated_at = DateTime.Now;
                    await _TargetRepository.Update(TargetToUpdate);
                    return "Successfully updated!";
                }
                else throw new TargetAlreadyExistsException();
            }
            catch { throw; }
        }

        #region Mappers

        private TargetInputDTO MapUpdateTargetDTOToTargetInputDTO(UpdateTargetInputDTO updateTargetInputDTO)
        {
            TargetInputDTO targetInputDTO = new TargetInputDTO();
            targetInputDTO.MetricType = updateTargetInputDTO.MetricType;
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
            targetOutputDTO.TargetStatus = target.TargetStatus;
            targetOutputDTO.TargetMinValue = target.TargetMinValue;
            targetOutputDTO.TargetMaxValue = target.TargetMaxValue;
            targetOutputDTO.TargetDate = target.TargetDate;
            return targetOutputDTO;
        }

        private Target MapTargetInputDTOToTarget(TargetInputDTO targetInputDTO, int prefId)
        {
            Target target = new Target();
            target.PreferenceId = prefId;
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
