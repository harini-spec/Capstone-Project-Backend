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
        private readonly IRepository<int, UserDetail> _UserDetailRepository;
        private readonly IEmailService _EmailService;

        public CoachService(IRepository<int, User> userRepository, IRepository<int, CoachCertificate> coachCertificateRepository, IRepository<int, UserDetail> userDetailRepository, IEmailService emailService)
        {
            _UserRepository = userRepository;
            _CoachCertificateRepository = coachCertificateRepository;
            _UserDetailRepository = userDetailRepository;
            _EmailService = emailService;
        }

        public async Task<string> ActivateCoach(int coachId)
        {
            try
            {
                var coachdetails = await _UserDetailRepository.GetById(coachId);
                if(coachdetails.Status == Models.ENUMs.UserStatusEnum.UserStatus.Active)
                {
                    throw new InvalidActionException("Account already activated!");
                }
                coachdetails.Status = Models.ENUMs.UserStatusEnum.UserStatus.Active;
                var coach = await _UserRepository.GetById(coachId);
                await _EmailService.SendEmailAsync(coach.Email, coach.Name);
                await _UserDetailRepository.Update(coachdetails);
                return "Successfully activated!";
            }
            catch { throw; }
        }

        public async Task<List<GetCoachDataDTO>> GetAllInactiveCoach()
        {
            try
            {
                var users = await _UserRepository.GetAll();
                var GetAllCoachList = users.Where(user => user.Role.ToString() == "Coach");
                var InactiveCoachList = new List<User>();

                foreach(var coach in GetAllCoachList)
                {
                    var userdetail = await _UserDetailRepository.GetById(coach.UserId);
                    if (userdetail.Status.ToString() == "Inactive")
                        InactiveCoachList.Add(coach);
                }
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
                    getCoachDataDTO.CoachId = user.UserId;
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
