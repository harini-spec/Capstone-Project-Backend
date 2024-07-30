namespace HealthTracker.Models.DTOs.Auth
{
    public class LoginOutputDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public bool IsPreferenceSet { get; set; }
    }
}
