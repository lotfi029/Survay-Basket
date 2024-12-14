using Survay_Basket.API.Contracts.Users;

namespace Survay_Basket.API.Controllers;
[Route("account")]
[ApiController]
[Authorize(Roles = DefaultRoles.User.Name)]
public class AcountsController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpGet("")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.GetUserId();

        var result = await _context.Users.GetProfileAsync(userId!);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("info")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.GetUserId();

        var result = await _context.Users.UpdateProfileAsync(userId!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.GetUserId();

        var result = await _context.Users.ChangePasswordAsync(userId!, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}