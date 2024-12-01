namespace Survay_Basket.API.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Result<PollResponse>> AddAsync(PollRequest model, CancellationToken cancellationToken = default)
    {
        var poll = model.Adapt<Poll>();

        if (poll == null)
            return null!;

        if (await _context.Polls.AnyAsync(e => e.Title == poll.Title, cancellationToken))
            return Result.Failure<PollResponse>(PollErrors.InvalidTitle);

        await _context.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var respone = poll.Adapt<PollResponse>();

        return Result.Success(respone);
    }
    public async Task<Result> UpdateAsync(
        int id, 
        PollRequest request, 
        CancellationToken cancellationToken = default
        )
    {
        var poll = await _context.Polls.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
        
        if (poll is null)
            return Result.Failure(PollErrors.NotFound);

        if (poll.Title != request.Title)
        {
            if (await _context.Polls.AnyAsync(e => e.Title == request.Title, cancellationToken: cancellationToken))
                return Result.Failure(PollErrors.InvalidTitle);
        }
        poll.Title = request.Title;
        poll.Summary = request.Description;
        
        poll.StartsAt = request.StartsAt;
        poll.EndsAt = request.EndsAt;

        await _context.SaveChangesAsync(cancellationToken); 

        return Result.Success();
    }
    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
        
        if (poll == null)
            return Result.Failure(PollErrors.NotFound);

        poll.IsPublished = !poll.IsPublished;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll == null)
            return Result.Failure(PollErrors.NotFound);

        _context.Polls.Remove(poll);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<List<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);
        
        if (polls is null)
            return Result.Failure<List<PollResponse>>(PollErrors.NotFound);

        
        return Result.Success(polls);
    }
    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls
            .Where(e => e.Id == id)
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        if (poll == null)
            return Result.Failure<PollResponse>(PollErrors.NotFound);

        return Result.Success(poll);
    }
    public async Task<Result<List<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {

        var currentPolls = await _context.Polls
            .Where(e => e.IsPublished 
            && e.StartsAt <= DateTime.UtcNow 
            && e.EndsAt >= DateTime.UtcNow)
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);

        if (currentPolls is null)
            return Result.Failure<List<PollResponse>>(PollErrors.NotFound);

        return Result.Success(currentPolls);
    }

}