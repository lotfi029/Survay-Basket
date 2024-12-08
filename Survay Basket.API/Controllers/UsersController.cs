using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survay_Basket.API.Contracts.Users;

namespace Survay_Basket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUnitOfWork userService) : ControllerBase
{
    private readonly IUnitOfWork _userService = userService;

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var result = await _userService.Users.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.Users.GetAllAsync();

        return Ok(result);
    }
    [HttpPost("")]
    [HasPermission(Permissions.AddUsers)]
    public async Task<IActionResult> Add([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.Users.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id} ,result.Value) : result.ToProblem();
    }
    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Update([FromRoute] string id,[FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.Users.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Toggle([FromRoute] string id)
    {
        var result = await _userService.Users.ToggleStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/unlock")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Unlock([FromRoute] string id)
    {
        var result = await _userService.Users.UnlockAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
