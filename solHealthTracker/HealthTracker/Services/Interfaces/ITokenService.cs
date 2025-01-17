﻿namespace HealthTracker.Services.Classes
{
    public interface ITokenService
    {
        #region Summary
        /// <summary>
        /// Generates JWT Token while registration of account and login check
        /// </summary>
        /// <typeparam name="T">Generic Object Type</typeparam>
        /// <param name="user">Generic Object</param>
        /// <returns>Generated Token string</returns>
        #endregion
        public Task<string> GenerateToken<T>(T user);
    }
}