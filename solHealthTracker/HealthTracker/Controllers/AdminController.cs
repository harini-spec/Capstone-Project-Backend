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
using Microsoft.AspNetCore.Authorization;

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


        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("ActivateCoach")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetAllInactiveCoaches(int coachId)
        {
            try
            {
                var result = await _CoachService.ActivateCoach(coachId);
                return Ok(result);
            }
            catch (InvalidActionException iae)
            {
                return UnprocessableEntity(new ErrorModel(422, iae.Message));
            }
            catch (EntityNotFoundException nif)
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
