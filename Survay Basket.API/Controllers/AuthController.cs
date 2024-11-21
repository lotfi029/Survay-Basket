using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Survay_Basket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        var authResult = await _context.AuthService.GetTokenAsync(request, cancellationToken);

        if (authResult is null) 
            return BadRequest();

        return Ok(authResult);
    }
}
