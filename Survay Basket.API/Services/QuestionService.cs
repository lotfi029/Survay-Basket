using Survay_Basket.API.Contracts.Answers;
using Survay_Basket.API.Contracts.Question;

namespace Survay_Basket.API.Services;

public class QuestionService(ApplicationDbContext context) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {

        var pollIsExits = await _context.Polls.AnyAsync(e => e.Id == pollId, cancellationToken);
        
        if (!pollIsExits)
            return Result.Failure<QuestionResponse>(PollErrors.NotFound);

        var questionIsExists = await _context.Questions.AnyAsync(e => e.Content == request.Content && e.PollId == pollId, cancellationToken);

        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;
        
        request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

        await _context.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = question.Adapt<QuestionResponse>();

        return Result.Success(response);
    }
    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
    {

        var questionIsExist = await _context.Questions
            .AnyAsync(e => 
            e.PollId == pollId 
            && e .Id != id 
            && e.Content == questionRequest.Content, 
            cancellationToken
            );

        if (questionIsExist)
            return Result.Failure(QuestionErrors.DuplicatedContent);

        var question = await _context.Questions
            .Include(e => e.Answers)
            .SingleOrDefaultAsync(e => e.PollId == pollId && e.Id == id);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFound);

        question.Content = questionRequest.Content;

        // current answers
        var curAnswers = question.Answers.Select(e => e.Content).ToList();

        // new answers
        var newAnswers = questionRequest.Answers.Except(curAnswers).ToList();

        newAnswers.ForEach(a => question.Answers.Add(new Answer { Content = a }));

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = questionRequest.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.SingleOrDefaultAsync(e => e.PollId == pollId && e.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result<QuestionResponse>> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Where(e => e.Id == id && e.PollId == pollId)
            .Include(e => e.Answers)
            .AsNoTracking()
            .ProjectToType<QuestionResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFound);

        var response = question.Adapt<QuestionResponse>();

        return Result.Success(response);
    }
    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions
            .Where(e => e.PollId == pollId)
            .Include(e => e.Answers)
            .AsNoTracking()
            .ProjectToType<QuestionResponse>()
            .ToListAsync(cancellationToken);

        if (questions is null || questions.Count == 0)
            return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.NotFound);

        var response = questions.Adapt<IEnumerable<QuestionResponse>>();

        return Result.Success(response);
    }
    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        var hasVote = await _context.Votes.AnyAsync(e => e.PollId == pollId && e.UserId == userId, cancellationToken);

        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);

        var pollIsExists = await _context.Polls.AnyAsync(e => 
            e.Id == pollId 
            && e.IsPublished
            && e.StartsAt <= DateTime.UtcNow
            && e.EndsAt >= DateTime.UtcNow,
            cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFound);

        var questions = await _context.Questions
            .Where(e => e.PollId == pollId && e.IsActive)
            .Include(e => e.Answers)
            .Select(q => new QuestionResponse(
                q.Id,
                q.Content,
                q.Answers.Where(e => e.IsActive).Select(e => new AnswerResponse(e.Id, e.Content))
                ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }
}
