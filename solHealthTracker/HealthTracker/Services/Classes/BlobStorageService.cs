using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HealthTracker.Services.Classes
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _containerName;
        private readonly IRepository<int, CoachCertificate> _CertificateRepository;

        public BlobStorageService(IRepository<int, CoachCertificate> CertificateRepository)
        {
            _containerName = "healthsyncblob";
            _CertificateRepository = CertificateRepository;
        }

        public async Task<string> GetConnectionString()
        {
            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var jwt_secret = await client.GetSecretAsync("HealthSyncBlobConnectionString");
            var secret = jwt_secret.Value.Value;
            return secret;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, int CoachId)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(await GetConnectionString());

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Create the container if it does not exist
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload the image stream to the blob
            await blobClient.UploadAsync(imageStream, true);

            CoachCertificate certificate = new CoachCertificate();
            certificate.CoachId = CoachId;
            certificate.CertificateURL = blobClient.Uri.ToString();
            await _CertificateRepository.Add(certificate);

            // Return the URI of the uploaded blob
            return blobClient.Uri.ToString();
        }
    }
}
