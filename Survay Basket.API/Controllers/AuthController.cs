using Azure;
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

        if (authResult.IsFailer)
            return authResult.ToProblem(StatusCodes.Status400BadRequest);

        return Ok(authResult.Value);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var refreshResult = await _context.AuthService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (refreshResult.IsFailer)
            return refreshResult.ToProblem(StatusCodes.Status400BadRequest);

        return Ok(refreshResult.Value);
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var isRevoked = await _context.AuthService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (isRevoked.IsFailer) 
            return isRevoked.ToProblem(StatusCodes.Status400BadRequest);

        return Ok();
    }
}
