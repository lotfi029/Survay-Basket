using Survay_Basket.API.Services;

namespace Survay_Basket.API;

public class UnitOfWork : IUnitOfWork
{
    public IPollService PollService { get; private set; }
    public UnitOfWork()
    {
        PollService = new PollService();
    }

    public void Dispose()
    {
        
    }
}
