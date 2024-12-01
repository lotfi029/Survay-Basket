namespace Survay_Basket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IUnitOfWork context) : ControllerBase
{
    private readonly IUnitOfWork _context = context;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        //throw new Exception("my exception");
        var result = await _context.AuthService.GetTokenAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var result = await _context.AuthService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var isRevoked = await _context.AuthService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked.IsSuccess ? Ok() : isRevoked.ToProblem();
    }
}
