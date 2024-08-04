namespace HealthTracker.Services.Interfaces
{
    public interface IBlobStorageService
    {
        public Task<string> UploadImageAsync(Stream imageStream, string fileName, int CoachId);
    }
}
