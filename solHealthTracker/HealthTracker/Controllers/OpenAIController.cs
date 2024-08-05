using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Graph;
using HealthTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Services.Interfaces;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _OpenAIService;

        public OpenAIController(IOpenAIService openAIService)
        {
            _OpenAIService = openAIService;
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetChatGPTResponseData")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetChatGPTResponseData(string Query)
        {
            try
            {
                var result = await _OpenAIService.GetCompletionAsync(Query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(500, ex.Message));
            }
        }
    }
}
