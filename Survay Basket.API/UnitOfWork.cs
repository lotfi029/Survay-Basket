using Microsoft.AspNetCore.Identity;
using Survay_Basket.API.Authentication;

namespace Survay_Basket.API;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public IPollService PollService { get; private set; }
    public IAuthService AuthService { get; private set; }
    public UnitOfWork(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider
        )
    {
        _context = context;
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        PollService = new PollService(_context);
        AuthService = new AuthService(_userManager, _jwtProvider);
    }

    public void Dispose() 
        => _context.Dispose();
}
