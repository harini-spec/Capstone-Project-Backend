using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Auth;

namespace HealthTracker.Services.Interfaces
{
    public interface IUserService
    {
        #region Summary 
        /// <summary>
        /// Get user object by Email ID
        /// </summary>
        /// <param name="email">Email ID string</param>
        /// <returns>User with given Email ID</returns>
        #endregion 
        public Task<User> GetUserByEmail(string email);

        #region Summary
        /// <summary>
        /// Register Coach and User accounts
        /// </summary>
        /// <param name="registerDTO">Account registration details DTO</param>
        /// <param name="Role">Coach/User</param>
        /// <returns>Registration status message</returns>
        #endregion
        public Task<string> RegisterUser(RegisterInputDTO registerDTO);

        #region Summary 
        /// <summary>
        /// Login Admin and Customer with Email and Password
        /// </summary>
        /// <param name="loginDTO">Login details DTO</param>
        /// <returns>Login result DTO with JWT Token</returns>
        #endregion
        public Task<LoginOutputDTO> LoginUser(LoginInputDTO loginDTO);
    }
}
