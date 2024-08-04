using HealthTracker.Models.DTOs.Graph;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DTOs.Coach;
using System.Collections.Generic;
using HealthTracker.Exceptions;
using HealthTracker.Models.DBModels;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICoachService _CoachService;

        public AdminController(ICoachService coachService)
        {
            _CoachService = coachService;
        }


        [HttpGet("GetAllInactiveCoaches")]
        [ProducesResponseType(typeof(List<GetCoachDataDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetCoachDataDTO>>> GetAllInactiveCoaches()
        {
            try
            {
                var result = await _CoachService.GetAllInactiveCoach();
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
    }
}
