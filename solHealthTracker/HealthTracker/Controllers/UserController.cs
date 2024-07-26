using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    [ExcludeFromCodeCoverage]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("LoginUser")]
        [ProducesResponseType(typeof(LoginOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<LoginOutputDTO>> Login(LoginInputDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.LoginUser(userLoginDTO);
                    return Ok(result);
                }
                catch (UnauthorizedUserException uae)
                {
                    return Unauthorized(new ErrorModel(401, uae.Message));
                }
                catch (UserNotActiveException uue)
                {
                    return Unauthorized(new ErrorModel(401, uue.Message));
                }
                catch (ArgumentNullException ane)
                {
                    return BadRequest(new ErrorModel(400, ane.Message));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel(500, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check the object");
        }

        [HttpPost("RegisterUser")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Register(RegisterInputDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string result = await _userService.RegisterUser(userDTO);
                    return Ok(result);
                }
                catch (UnableToRegisterException ure)
                {
                    return Conflict(new ErrorModel(409, ure.Message));
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
