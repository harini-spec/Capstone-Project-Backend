using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HealthTracker.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepo;
        private readonly IRepository<int, UserDetail> _userDetailRepo;
        private readonly ITokenService _tokenService;


        public UserService(IRepository<int, User> userRepo, IRepository<int, UserDetail> userDetailRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _userDetailRepo = userDetailRepo;
            _tokenService = tokenService;
        }

        #region GetUserByEmail

        // Get User object by Email ID
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var users = await _userRepo.GetAll();
                var user = users.ToList().FirstOrDefault(x => x.Email == email);
                return user;
            }
            catch (NoItemsFoundException)
            {
                return null;
            }
        }

        #endregion


        #region LoginUser

        public async Task<LoginOutputDTO> LoginUser(LoginInputDTO loginInputDTO)
        {
            try
            {
                // Checking if Email ID is present 
                User user = await GetUserByEmail(loginInputDTO.Email);
                if (user == null)
                {
                    throw new UnauthorizedUserException("Invalid username or password");
                }

                UserDetail userDetail = await _userDetailRepo.GetById(user.UserId);
                if(userDetail == null)
                    throw new UnauthorizedUserException("Invalid username or password");

                HMACSHA512 hMACSHA = new HMACSHA512(userDetail.PasswordHashKey);
                var encryptedPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginInputDTO.Password));
                bool isPasswordSame = ComparePassword(encryptedPass, userDetail.PasswordEncrypted);

                // Checking if password is correct
                if (isPasswordSame)
                {
                    // Checking if account is active
                    if (userDetail.Status.ToString() == "Active")
                    {
                        LoginOutputDTO loginOutputDTO = await MapUserToLoginOutputDTO(user);
                        return loginOutputDTO;
                    }
                    throw new UserNotActiveException("Your account is not activated yet");
                }
                throw new UnauthorizedUserException("Invalid username or password");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ComparePassword(byte[] encryptedPass, byte[] passwordEncrypted)
        {
            for (int i = 0; i < encryptedPass.Length; i++)
            {
                if (encryptedPass[i] != passwordEncrypted[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region Register User

        public async Task<string> RegisterUser(RegisterInputDTO registerInputDTO)
        {
            // Checking for duplicate value - Email ID 
            var ExistingUser = await GetUserByEmail(registerInputDTO.Email);
            if (ExistingUser != null)
            {
                throw new UnableToRegisterException("Email ID already exists");
            }

            User user = null;
            User InsertedUser = null;
            UserDetail userDetail = null;
            UserDetail InsertedUserDetail = null;

            try
            {
                userDetail = MapRegisterInputDTOToUserDetail(registerInputDTO);
                InsertedUserDetail = await _userDetailRepo.Add(userDetail);

                user = MapRegisterInputDTOToUser(registerInputDTO);
                user.UserId = InsertedUserDetail.Id;
                InsertedUser = await _userRepo.Add(user);

                return "Registered Successfully!";
            }
            catch (Exception)
            {
                if (InsertedUser == null && InsertedUserDetail != null)
                {
                    await RevertUserDetailInsert(InsertedUserDetail.Id);
                    throw new UnableToRegisterException("Not able to register at this moment");
                }
                throw new UnableToRegisterException("Not able to register at this moment");
            }
        }

        private async Task RevertUserDetailInsert(int userId)
        {
            await _userDetailRepo.Delete(userId);
        }

        #endregion


        #region Update User

        public async Task UpdateUser(User user)
        {
            try
            {
                await _userRepo.Update(user);
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region Get User By Id

        public async Task<User> GetUserById(int Id)
        {
            try
            {
                return await _userRepo.GetById(Id);
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region Mappers

        private async Task<LoginOutputDTO> MapUserToLoginOutputDTO(User user)
        {
            LoginOutputDTO loginOutputDTO = new LoginOutputDTO();

            loginOutputDTO.UserID = user.UserId;
            loginOutputDTO.UserName = user.Name;
            loginOutputDTO.Role = user.Role.ToString();
            loginOutputDTO.Token = await _tokenService.GenerateToken(user);
            loginOutputDTO.IsPreferenceSet = user.is_preferenceSet;

            return loginOutputDTO;
        }

        private UserDetail MapRegisterInputDTOToUserDetail(RegisterInputDTO registerInputDTO)
        {
            UserDetail userDetail = new UserDetail();

            HMACSHA512 hMACSHA = new HMACSHA512();
            userDetail.PasswordHashKey = hMACSHA.Key;
            userDetail.PasswordEncrypted = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(registerInputDTO.Password));

            userDetail.Status = UserStatusEnum.UserStatus.Active;

            //if (registerInputDTO.Role.ToString() == "User")
            //    userDetail.Status = UserStatusEnum.UserStatus.Active;
            //else if (registerInputDTO.Role.ToString() == "Coach")
            //    userDetail.Status = UserStatusEnum.UserStatus.Inactive;

            userDetail.Created_at = DateTime.Now;
            userDetail.Updated_at = DateTime.Now;

            return userDetail;
        }

        private User MapRegisterInputDTOToUser(RegisterInputDTO registerInputDTO)
        {
            User user = new User();
            Enum.TryParse(registerInputDTO.Role, out RolesEnum.Roles role);
            Enum.TryParse(registerInputDTO.Gender, out GenderEnum.Gender gender);

            user.Name = registerInputDTO.Name;
            user.Age = registerInputDTO.Age;
            user.Gender = gender;
            user.Phone = registerInputDTO.Phone;
            user.Email = registerInputDTO.Email;
            user.Role = role;
            user.Created_at = DateTime.Now;
            user.Updated_at = DateTime.Now;

            return user;
        }

        #endregion
    }
}
