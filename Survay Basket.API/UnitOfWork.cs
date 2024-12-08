using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Survay_Basket.API.Settings;

namespace Survay_Basket.API;

public class UnitOfWork(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IRoleService roleService) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;


    public IPollService PollService { get; private set; } = new PollService(context);
    //public IAuthService AuthService { get; private set; } = new AuthService(userManager,jwtProvider, signInManager,emailSender, httpContextAccessor,mailSettings);
    public IQuestionService QuestionService { get; private set; } = new QuestionService(context);
    public IVoteService VoteService { get; private set; } = new VoteService(context);
    public IResultService ResultService { get; private set; } = new ResultService(context);
    public Services.IUserService Users { get; private set; } = new UserService(userManager, context, roleService);

    public void Dispose() 
        => _context.Dispose();

    public Task<int> CommitChanges(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
}
