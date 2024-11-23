using Azure;
using Microsoft.AspNetCore.Authorization;
using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _context.PollService.GetAllAsync(cancellationToken);

        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status404NotFound);

        return Ok(response.Value);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.GetByIdAsync(id, cancellationToken);

        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status404NotFound);

        return Ok(response.Value);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, 
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.AddAsync(request, cancellationToken);

        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(Get),new { id = response.Value.Id}, response.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.UpdateAsync(id, request, cancellationToken);

        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status400BadRequest);

        return Ok(response.Value);
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.TogglePublishStatusAsync(id, cancellationToken);
        
        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status400BadRequest);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.DeleteAsync(id, cancellationToken);
        
        if (response.IsFailer)
            return response.ToProblem(StatusCodes.Status400BadRequest);

        return Ok();
    }
}
