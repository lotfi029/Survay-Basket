namespace Survay_Basket.API.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]

public class ResultsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpGet("row-data")]
    public async Task<IActionResult> GetPollVotes([FromRoute]int pollId, CancellationToken cancellationToken)
    {
        var result = await _context.ResultService.GetPollVotesAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("votes-per-day")]
    public async Task<IActionResult> GetVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _context.ResultService.GetVotePerDayResponse(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("votes-per-question")]
    public async Task<IActionResult> GetVotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _context.ResultService.GetVotePerQuestionResponse(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
