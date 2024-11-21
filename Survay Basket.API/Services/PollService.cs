using Mapster;
using Survay_Basket.API.Contracts.Polls;

namespace Survay_Basket.API.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<PollResponse> AddAsync(PollRequest model, CancellationToken cancellationToken = default)
    {
        var poll = model.Adapt<Poll>();

        if (poll == null)
            return null!;

        if (await _context.Polls.AnyAsync(e => e.Title == poll.Title))
            return null!;

        await _context.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return poll.Adapt<PollResponse>();
    }
    public async Task<PollResponse> UpdateAsync(int id, 
        PollRequest request, 
        CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll is null)
            return null!;
        if (poll.Title != request.Title)
        {
            if (await _context.Polls.AnyAsync(e => e.Title == request.Title))
                return null!;
        }
        poll.Title = request.Title;
        poll.Summary = request.Description;
        poll.IsPublished = request.IsPublished;
        poll.StartsAt = request.StartsAt;
        poll.EndsAt = request.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);

        return poll.Adapt<PollResponse>();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll == null) return false;
        
        _context.Polls.Remove(poll);
        
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
        
        if (polls is null)
            return null!;

        return polls.Adapt<IEnumerable<PollResponse>>();            
    }
    public async Task<PollResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll == null)
            return null!;

        return poll.Adapt<PollResponse>();
    }
    public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        
        if (poll == null) 
            return false;

        poll.IsPublished = !poll.IsPublished;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}