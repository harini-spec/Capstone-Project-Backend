using HealthTracker.Exceptions;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.DTOs;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HealthTracker.Models.DTOs.Suggestions;

namespace HealthTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly IProblemService _ProblemService;

        public ProblemController(IProblemService problemService)
        {
            _ProblemService = problemService;
        }

        [Authorize(Roles = "Coach")]
        [HttpGet("GetProblems")]
        [ProducesResponseType(typeof(List<ProblemOutputDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProblemOutputDTO>>> GetProblems()
        {
                try
                {
                    int CoachId = -1;
                    foreach (var claim in User.Claims)
                    {
                        if (claim.Type == "ID")
                            CoachId = Convert.ToInt32(claim.Value);
                    }
                    var result = await _ProblemService.GetUserIdsWithProblems(CoachId);
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

        [Authorize(Roles = "Coach")]
        [HttpPost("AddSuggestion")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddSuggestion(SuggestionInputDTO suggestionDTO)
        {
            try
            {
                int CoachId = -1;
                foreach (var claim in User.Claims)
                {
                    if (claim.Type == "ID")
                        CoachId = Convert.ToInt32(claim.Value);
                }
                var result = await _ProblemService.AddSuggestion(suggestionDTO, CoachId);
                return Ok(result);
            }
            catch (InvalidOperationException ioe)
            {
                return UnprocessableEntity(new ErrorModel(422, ioe.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(500, ex.Message));
            }
        }
    }
}
