using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DTOs.GoogleFit;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors;

namespace HealthTracker.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class OAuthController : ControllerBase
    {
        private readonly IOAuthTokenService _OAuthTokenService;

        public OAuthController(IOAuthTokenService oAuthTokenService)
        {
            _OAuthTokenService = oAuthTokenService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddOrUpdateOAuthAccessToken")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddOrUpdateOAuthAccessToken(OAuthAccessTokenDTO accessTokenDTO)
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
                    var result = await _OAuthTokenService.AddOrUpdateAccessTokenToDB(accessTokenDTO, UserId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check the object");
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetValidOAuthAccessToken")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OAuthAccessTokenDTO>> GetValidOAuthAccessToken()
        {
                try
                {
                    int UserId = -1;
                    foreach (var claim in User.Claims)
                    {
                        if (claim.Type == "ID")
                            UserId = Convert.ToInt32(claim.Value);
                    }
                    var result = await _OAuthTokenService.GetAccessTokenDTO(UserId);
                    return Ok(result);
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
        [HttpGet("GetOAuthCreds")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OAuthCredsResponseDTO>> GetOAuthCreds()
        {
            try
            {
                var result = await _OAuthTokenService.GetOAuthCreds();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(500, ex.Message));
            }
        }
    }
}
