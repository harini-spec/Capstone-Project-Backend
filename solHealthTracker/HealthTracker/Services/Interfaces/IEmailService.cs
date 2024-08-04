namespace HealthTracker.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string CoachName);
    }
}
