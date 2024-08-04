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
using Microsoft.AspNetCore.Cors;

namespace HealthTracker.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
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

        [Authorize(Roles = "User, Coach")]
        [HttpGet("GetGraphDataRange")]
        [ProducesResponseType(typeof(GraphDataRangeOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GraphDataRangeOutputDTO>> GraphDataRangeOutputDTO(string MetricType)
        {
            try
            {
                int UserId = -1;
                foreach (var claim in User.Claims)
                {
                    if (claim.Type == "ID")
                        UserId = Convert.ToInt32(claim.Value);
                }
                var result = await _GraphService.GetGraphDataHealthyRange(MetricType, UserId);
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
