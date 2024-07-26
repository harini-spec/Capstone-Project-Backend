using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Target;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class TargetController : ControllerBase
    {
        private readonly ITargetService _TargetService;

        public TargetController(ITargetService targetService)
        {
            _TargetService = targetService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddTarget")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<string>> AddTarget(TargetInputDTO targetInputDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _TargetService.AddTarget(targetInputDTO);
                    return Ok(result);
                }
                catch (InvalidOperationException ioe)
                {
                    return UnprocessableEntity(new ErrorModel(422, ioe.Message));
                }
                catch (NoItemsFoundException nif)
                {
                    return NotFound(new ErrorModel(404, nif.Message));
                }
                catch (EntityNotFoundException enf)
                {
                    return NotFound(new ErrorModel(404, enf.Message));
                }
                catch (EntityAlreadyExistsException tae)
                {
                    return Conflict(new ErrorModel(409, tae.Message));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check the object");
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetTargetForToday")]
        [ProducesResponseType(typeof(TargetOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TargetOutputDTO>> GetTargetForToday(int PrefId)
        {
                try
                {
                    int UserId = -1;
                    foreach (var claim in User.Claims)
                    {
                        if (claim.Type == "ID")
                            UserId = Convert.ToInt32(claim.Value);
                    }
                    var result = await _TargetService.GetTodaysTarget(PrefId, UserId);
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

        [Authorize(Roles = "User")]
        [HttpPut("UpdateTarget")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<string>> UpdateTarget(UpdateTargetInputDTO targetInputDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _TargetService.UpdateTarget(targetInputDTO);
                    return Ok(result);
                }
                catch (InvalidOperationException ioe)
                {
                    return UnprocessableEntity(new ErrorModel(422, ioe.Message));
                }
                catch (NoItemsFoundException nif)
                {
                    return NotFound(new ErrorModel(404, nif.Message));
                }
                catch (EntityNotFoundException enf)
                {
                    return NotFound(new ErrorModel(404, enf.Message));
                }
                catch (EntityAlreadyExistsException tae)
                {
                    return Conflict(new ErrorModel(409, tae.Message));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check the object");
        }
    }
}
