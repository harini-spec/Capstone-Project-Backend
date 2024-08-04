using HealthTracker.Models.DTOs.Graph;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Services.Classes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors;

namespace HealthTracker.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CoachController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public CoachController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }


        [HttpPost("UploadCertificate")]
        [ProducesResponseType(typeof(List<GraphDataOutputDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UploadCertificate(IFormFile Certificate, int CoachId)
        {
            if (Certificate == null || Certificate.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            using (var stream = Certificate.OpenReadStream())
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Certificate.FileName);
                string imageUrl = await _blobStorageService.UploadImageAsync(stream, fileName, CoachId);
                return Ok(new { imageUrl });
            }
        }
    }
}
