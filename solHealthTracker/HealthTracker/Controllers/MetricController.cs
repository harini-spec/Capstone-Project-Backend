using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using HealthTracker.Models.DTOs.MetricPreference;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    [ExcludeFromCodeCoverage]
    public class MetricController : ControllerBase
    {
        private readonly IMetricService _MetricService;

        public MetricController(IMetricService metricService)
        {
            _MetricService = metricService;
        }

        [HttpGet("GetAllMetrics")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<List<string>>> GetAllMetrics()
        {
                try
                {
                    var result = await _MetricService.GetAllMetrics();
                    return Ok(result);
                }
                catch (NoItemsFoundException nif)
                {
                    return NotFound(new ErrorModel(404, nif.Message));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
        }

        [Authorize(Roles = "User, Coach")]
        [HttpGet("GetPreferenceListOfUser")]
        [ProducesResponseType(typeof(List<PreferenceOutputDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PreferenceOutputDTO>>> GetPreferencesListOfUser()
        {
            try
            {
                int UserId = -1;
                string Role = "";
                foreach (var claim in User.Claims)
                {
                    if (claim.Type == "ID")
                        UserId = Convert.ToInt32(claim.Value);
                    if (claim.Type.Contains("role"))
                        Role = claim.Value;
                }
                var result = await _MetricService.GetPreferencesListOfUser(UserId, Role);
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
        [HttpPost("AddPreferenceListOfUser")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddPreferenceList(List<string> Preferences)
        {
            try
            {
                int UserId = -1;
                string Role = "";
                foreach (var claim in User.Claims)
                {
                    if (claim.Type == "ID")
                        UserId = Convert.ToInt32(claim.Value);
                    if(claim.Type.Contains("role"))
                        Role = claim.Value;
                }
                var result = await _MetricService.AddPreference(Preferences, UserId, Role);
                return Ok(result);
            }
            catch (UnauthorizedUserException uae)
            {
                return Unauthorized(new ErrorModel(401, uae.Message));
            }
            catch (NoItemsFoundException nif)
            {
                return NotFound(new ErrorModel(404, nif.Message));
            }
            catch (EntityNotFoundException enf)
            {
                return NotFound(new ErrorModel(404, enf.Message));
            }
            catch(EntityAlreadyExistsException eae)
            {
                return Conflict(new ErrorModel(404, eae.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(500, ex.Message));
            }
        }
    }
}
