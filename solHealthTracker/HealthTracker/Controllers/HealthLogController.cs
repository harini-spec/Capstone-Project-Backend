﻿using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DTOs.HealthLog;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthLogController : ControllerBase
    {
        private readonly IHealthLogService _HealthLogService;

        public HealthLogController(IHealthLogService healthLogService)
        {
            _HealthLogService = healthLogService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddHealthLog")]
        [ProducesResponseType(typeof(HealthLogOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<HealthLogOutputDTO>> AddHealthLog(HealthLogInputDTO healthLogInputDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int UserId = -1;
                    foreach (var claim in User.Claims)
                    {
                        if (claim.Type == "ID")
                            UserId = Convert.ToInt32(claim.Value);
                    }
                    var result = await _HealthLogService.AddHealthLog(healthLogInputDTO, UserId);
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
