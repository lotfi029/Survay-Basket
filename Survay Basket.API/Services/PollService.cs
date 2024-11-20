namespace Survay_Basket.API.Services;

public class PollService : IPollService
{

    private List<Poll> _polls = [
        new()
        {
            Id = 1,
            Title = "Poll 1",
            Description = "My FIrst Poll"
        }
        ];

    public Poll Add(Poll poll)
    {
        var pollId = _polls.Count + 1;

        poll.Id = pollId;
        _polls.Add(poll);

        return poll;
    }
    public bool Update(int id, Poll poll)
        => _polls.Any(e => e.Id == id);

    public bool Delete(int id) => _polls.Any(e => e.Id == id);
    public ICollection<Poll> GetAll() => _polls;
    public Poll? GetById(int id) => _polls.SingleOrDefault(e => e.Id == id);
}