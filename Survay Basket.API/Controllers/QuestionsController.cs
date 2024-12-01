using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survay_Basket.API.Contracts.Question;
using Survay_Basket.API.Errors;
using System.Threading;

namespace Survay_Basket.API.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId,[FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _context.QuestionService.AddAsync(pollId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {pollId, id = result.Value.Id},result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest question, CancellationToken cancellationToken)
    {
        var result = await _context.QuestionService.UpdateAsync(pollId, id, question, cancellationToken);



        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{id}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _context.QuestionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId,[FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _context.QuestionService.GetByIdAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _context.QuestionService.GetAllAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
