using Mapster;
using Survay_Basket.API.Contracts.Polls;
using Survay_Basket.API.Errors;

namespace Survay_Basket.API.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Result<PollResponse>> AddAsync(PollRequest model, CancellationToken cancellationToken = default)
    {
        var poll = model.Adapt<Poll>();

        if (poll == null)
            return null!;

        if (await _context.Polls.AnyAsync(e => e.Title == poll.Title))
            return Result.Failure<PollResponse>(PollErrors.InvalidTitle);

        await _context.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var respone = poll.Adapt<PollResponse>();

        return Result.Success(respone);
    }
    public async Task<Result<PollResponse>> UpdateAsync(int id, 
        PollRequest request, 
        CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll is null)
            return Result.Failure<PollResponse>(PollErrors.NotFound);

        if (poll.Title != request.Title)
        {
            if (await _context.Polls.AnyAsync(e => e.Title == request.Title))
                return Result.Failure<PollResponse>(PollErrors.InvalidTitle);
        }
        poll.Title = request.Title;
        poll.Summary = request.Description;
        
        poll.StartsAt = request.StartsAt;
        poll.EndsAt = request.EndsAt;

        await _context.SaveChangesAsync(cancellationToken); 

        var response = poll.Adapt<PollResponse>();

        return Result.Success(response);
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

    public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
        
        if (polls is null)
            return Result.Failure<IEnumerable<PollResponse>>(PollErrors.NotFound);

        var response = polls.Adapt<IEnumerable<PollResponse>>();            
        
        return Result.Success(response);
    }
    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll == null)
            return Result.Failure<PollResponse>(PollErrors.NotFound);

        var respone = poll.Adapt<PollResponse>();

        return Result.Success(respone);
    }
    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll == null)
            return Result.Failure(PollErrors.NotFound);

        poll.IsPublished = !poll.IsPublished;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}