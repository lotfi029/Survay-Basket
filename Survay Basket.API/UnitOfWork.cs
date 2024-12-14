using Microsoft.Extensions.Caching.Hybrid;

namespace Survay_Basket.API;

public class UnitOfWork(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IRoleService roleService,
    INotificationService notificationService,
    HybridCache hybridCache) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public IPollService PollService { get; private set; } = new PollService(context,notificationService);
    public IQuestionService QuestionService { get; private set; } = new QuestionService(context, hybridCache);
    public IVoteService VoteService { get; private set; } = new VoteService(context);
    public IResultService ResultService { get; private set; } = new ResultService(context);
    public IUserService Users { get; private set; } = new UserService(userManager, context, roleService);

    public void Dispose() 
        => _context.Dispose();

    public Task<int> CommitChanges(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
}
