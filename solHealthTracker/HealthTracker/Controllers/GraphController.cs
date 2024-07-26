using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DTOs.Graph;
using System.Diagnostics.CodeAnalysis;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class GraphController : ControllerBase
    {
        private readonly IGraphService _GraphService;

        public GraphController(IGraphService graphService)
        {
            _GraphService = graphService;
        }

        [Authorize(Roles = "User, Coach")]
        [HttpGet("GetGraphData")]
        [ProducesResponseType(typeof(List<GraphDataOutputDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GraphDataOutputDTO>>> GetGraphData(string MetricType, string Duration, int UserId)
        {
                try
                {
                    var result = await _GraphService.GetGraphData(MetricType, Duration, UserId);
                    return Ok(result);
                }
                catch (NoItemsFoundException nif)
                {
                    return NotFound(new ErrorModel(404, nif.Message));
                }
                catch (EntityNotFoundException enf)
                {
                    return NotFound(new ErrorModel(404, enf.Message));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
        }
    }
}
