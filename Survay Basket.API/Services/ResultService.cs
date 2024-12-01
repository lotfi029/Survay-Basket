using Survay_Basket.API.Contracts.Results;

namespace Survay_Basket.API.Services;

public class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollVotes = await _context.Polls
            .Where(e => e.Id == pollId)
            .Select(p => new PollVotesResponse(
                p.Title,
                p.Votes.Select(v => 
                new VoteResponse (
                    $"{v.User.FirstName} {v.User.LastName}", 
                    v.SubmittedOn, 
                    v.Answers.Select( q => new QuestionAnswersResponse(
                        q.Question.Content,
                        q.Answer.Content
                    ))
                ))
            )).SingleOrDefaultAsync(cancellationToken);

        return pollVotes == null ? Result.Failure<PollVotesResponse>(PollErrors.NotFound) : Result.Success(pollVotes);
    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotePerDayResponse(int pollId, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(e => e.Id == pollId, cancellationToken);
         
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.NotFound);

        var votesPerDay = await _context.Votes
            .Where(e => e.PollId == pollId)
            .GroupBy(x => new {Date = DateOnly.FromDateTime(x.SubmittedOn)})
            .Select(x => new VotesPerDayResponse(x.Key.Date, x.Count()))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
    } 
    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotePerQuestionResponse(int pollId, CancellationToken cancellationToken = default)
    {
        var pollExists = await _context.Polls.AnyAsync(e => e.Id == pollId, cancellationToken);
         
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.NotFound);

        var votesPerQuestion = await _context.VoteAnswers
            .Where(e => e.Vote.PollId == pollId)
            .Select(x => new VotesPerQuestionResponse(
                x.Question.Content, 
                x.Question.VoteAnswers
                .GroupBy(e => new {AnswerId = e.Answer.Id, AnswerContent = e.Answer.Content})
                .Select(x => new VotesPerAnswersResponse(x.Key.AnswerContent,x.Count()))
                )
            ).ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
    } 
}
