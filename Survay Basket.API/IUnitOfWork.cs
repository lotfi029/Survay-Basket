namespace Survay_Basket.API;

public interface IUnitOfWork : IDisposable
{
    IPollService PollService { get; }
    //IAuthService AuthService { get; }
    IQuestionService QuestionService { get; }
    IVoteService VoteService { get; }
    IResultService ResultService { get; }
    IUserService Users { get; }

    Task<int> CommitChanges(CancellationToken cancellationToken);
}
