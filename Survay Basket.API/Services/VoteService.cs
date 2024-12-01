using Survay_Basket.API.Contracts.Votes;

namespace Survay_Basket.API.Services;

public class VoteService(ApplicationDbContext context) : IVoteService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken)
    {
        var hasVote = await _context.Votes
            .AnyAsync(e => e.PollId == pollId && e.UserId == userId, cancellationToken);
            
        if (hasVote)
            return Result.Failure(VoteErrors.DuplicatedVote);

        var pollIsExists = await _context.Polls
            .AnyAsync(
            e => e.Id == pollId
            && e.IsPublished
            && e.StartsAt <= DateTime.UtcNow
            && e.EndsAt >= DateTime.UtcNow
            , cancellationToken);

        if (!pollIsExists)
            return Result.Failure(PollErrors.NotFound);


        var curQuestions = await _context.Questions
            .Where(e => e.PollId == pollId && e.IsActive)
            .AsNoTracking()
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (!request.Answers.Select(e => e.QuestionId).SequenceEqual(curQuestions))
            return Result.Failure(VoteErrors.InvalidQuestions);


        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            Answers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };
        await _context.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }

}
