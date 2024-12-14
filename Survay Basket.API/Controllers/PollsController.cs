using Asp.Versioning;
using Microsoft.AspNetCore.OutputCaching;
using Survay_Basket.API.Abstractions.Consts;

namespace Survay_Basket.API.Controllers;

[ApiVersion(1 , Deprecated = true)]
[ApiVersion(2)]
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class PollsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.AddAsync(request, cancellationToken);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(Get),new { id = result.Value.Id}, result.Value) 
            : result.ToProblem(); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ?  NoContent() : result.ToProblem();
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.TogglePublishStatusAsync(id, cancellationToken);
        
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.DeleteAsync(id, cancellationToken);
        
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _context.PollService.GetAllAsync(cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [MapToApiVersion(1)]
    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.User.Name)]
    public async Task<IActionResult> GetCurrentV1(CancellationToken cancellationToken)
    {
        var result = await _context.PollService.GetCurrentAsync(cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [MapToApiVersion(2)]
    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.User.Name)]
    public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
    {
        var result = await _context.PollService.GetCurrentAsync(cancellationToken);

        return result.IsSuccess ? Ok(result.Value.Select(e => new {e.Id, e.Message, e.StartsAt, e.EndsAt })) : result.ToProblem();
    }
}
