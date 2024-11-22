using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Survay_Basket.API.Authentication;

namespace Survay_Basket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        var authResult = await _context.AuthService.GetTokenAsync(request, cancellationToken);

        if (authResult is null) 
            return BadRequest("Invalid email | Password");

        return Ok(authResult);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var refreshResult = await _context.AuthService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (refreshResult is null) 
            return BadRequest("Invalid Token");

        return Ok(refreshResult);
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var isRevoked = await _context.AuthService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (!isRevoked) 
            return BadRequest("Operation Faild");

        return Ok();
    }
}
