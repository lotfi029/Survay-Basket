using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Survay_Basket.API.Abstractions;
using Survay_Basket.API.Contracts.Votes;

namespace Survay_Basket.API.Controllers;
[Route("api/polls/{pollId}/vote")]
[ApiController]
[EnableRateLimiting(RateLimiterPolicyNames.Concurrency)]
[Authorize]
public class VotesController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _unitOfWork.VoteService.AddAsync(pollId, userId!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _unitOfWork.QuestionService.GetAvailableAsync(pollId, userId!, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
