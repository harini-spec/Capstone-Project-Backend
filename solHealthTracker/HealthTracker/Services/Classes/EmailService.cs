using MimeKit;
using MailKit.Net.Smtp;
using HealthTracker.Services.Interfaces;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace HealthTracker.Services.Classes
{
    public class EmailService : IEmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;

        private async Task<string> GetEmailID()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("HealthSyncEmailID");
            var secret = jwt_secret.Value.Value;
            return secret;
        }

        private async Task<string> GetEmailPWD()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("HealthSyncEmailPWD");
            var secret = jwt_secret.Value.Value;
            return secret;
        }

        public async Task SendEmailAsync(string toEmail, string CoachName)
        {
            string SmtpUser = await GetEmailID();
            string SmtpPass = await GetEmailPWD();

            var subject = "Your Coach Account is Activated!";
            var body = $"Hello {CoachName}!! Your Coach Account in HealthSync is approved and is currently active. You can start providing people your feedback and suggestions! Hope you enjoy your time with HealthSync!";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HealthSync", SmtpUser));
            message.To.Add(new MailboxAddress("Coach", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine("Connecting to SMTP server...");
                    await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    Console.WriteLine("Authenticating...");
                    await client.AuthenticateAsync(SmtpUser, SmtpPass);
                    Console.WriteLine("Sending email...");
                    await client.SendAsync(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caught in SendEmailAsync(): {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("Disconnecting from SMTP server...");
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
