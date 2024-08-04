using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Coach;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Services.Classes
{
    public class CoachService : ICoachService
    {
        private readonly IRepository<int, User> _UserRepository;
        private readonly IRepository<int, CoachCertificate> _CoachCertificateRepository;

        public CoachService(IRepository<int, User> userRepository, IRepository<int, CoachCertificate> coachCertificateRepository)
        {
            _UserRepository = userRepository;
            _CoachCertificateRepository = coachCertificateRepository;
        }

        public async Task<List<GetCoachDataDTO>> GetAllInactiveCoach()
        {
            try
            {
                var users = await _UserRepository.GetAll();
                var InactiveCoachList = users.Where(user => user.Role.ToString() == "Coach" && user.UserDetailsForUser.Status.ToString() == "Inactive");
                if (InactiveCoachList.ToList().Count == 0)
                    throw new NoItemsFoundException("No Inactive coaches found!");
                return await MapCoachDataToCoachDTO(InactiveCoachList.ToList());
            }
            catch { throw; }
        }

        private async Task<List<GetCoachDataDTO>> MapCoachDataToCoachDTO(List<User> data)
        {
            try
            {
                List<GetCoachDataDTO> result = new List<GetCoachDataDTO>();
                foreach (var user in data)
                {
                    GetCoachDataDTO getCoachDataDTO = new GetCoachDataDTO();
                    getCoachDataDTO.Name = user.Name;
                    getCoachDataDTO.Age = user.Age;
                    getCoachDataDTO.Gender = user.Gender.ToString();
                    getCoachDataDTO.Phone = user.Phone;
                    getCoachDataDTO.Email = user.Email;
                    List<CoachCertificate> certificates = new List<CoachCertificate>();

                    try
                    {
                        certificates = await _CoachCertificateRepository.GetAll();
                        var certificate = certificates.Where(c => c.CoachId == user.UserId).ToList();
                        getCoachDataDTO.Certificate = certificate[0].CertificateURL;
                    }
                    catch { }

                    result.Add(getCoachDataDTO);
                }
                return result;
            }
            catch { throw; }
        }
    }
}
