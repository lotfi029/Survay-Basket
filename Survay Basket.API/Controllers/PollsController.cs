using Microsoft.AspNetCore.Authorization;
using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _context.PollService.GetAllAsync(cancellationToken);

        

        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var response = await _context.PollService.GetByIdAsync(id, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, 
        CancellationToken cancellationToken)
    {
        // Without Using AddFluentValidationAutoValidation
        ///var validationResult = validator.Validate(request);
        ///if (!validationResult.IsValid)
        ///{
        ///    ModelStateDictionary modelState = new();
        ///    validationResult.Errors.ForEach(e => modelState.AddModelError( e.PropertyName, e.ErrorMessage ));
        ///    return ValidationProblem();
        ///}

        var result = await _context.PollService.AddAsync(request, cancellationToken);

        if (result == null)
            return BadRequest("This Title Exists!");

        var respone = result.Adapt<PollResponse>();


        return CreatedAtAction(nameof(Get), new {id = result.Id}, respone);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, 
        [FromBody] PollRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.UpdateAsync(id, request, cancellationToken);
        
        if (result is null)
            return BadRequest("Not Found");

        return Ok(result);
    }
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var toggleResponse = await _context.PollService.TogglePublishStatusAsync(id, cancellationToken);
        if (!toggleResponse)
            return NotFound();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var result = await _context.PollService.DeleteAsync(id, cancellationToken);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
