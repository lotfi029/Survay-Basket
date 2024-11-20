namespace Survay_Basket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpGet]
    public IActionResult GetAll()
    {
        var polls = _context.PollService.GetAll();

        var respone = polls.Adapt<List<PollResponse>>();

        return Ok(respone);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _context.PollService.GetById(id);

        if (poll == null)
            return NotFound();

        var respone = poll.Adapt<PollResponse>();

        return Ok(_context.PollService.GetById(id));
    }

    [HttpPost("")]
    public IActionResult Add([FromBody] CreatePollRequest request)
    {
        // Without Using AddFluentValidationAutoValidation
        ///var validationResult = validator.Validate(request);
        ///if (!validationResult.IsValid)
        ///{
        ///    ModelStateDictionary modelState = new();
        ///    validationResult.Errors.ForEach(e => modelState.AddModelError( e.PropertyName, e.ErrorMessage ));
        ///    return ValidationProblem();
        ///}

        var result = _context.PollService.Add(request.Adapt<Poll>());

        var respone = result.Adapt<PollResponse>();


        return CreatedAtAction(nameof(Get), new {id = result.Id}, respone);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Poll request)
    {
        var result = _context.PollService.Update(id, request);
        
        if (!result)
            return NotFound();

        return NoContent();
    }
    [HttpDelete("")]
    public IActionResult Delete([FromRoute] int id)
    {
        var result = _context.PollService.Delete(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
