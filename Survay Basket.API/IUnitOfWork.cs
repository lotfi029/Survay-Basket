using Survay_Basket.API.Services;

namespace Survay_Basket.API;

public interface IUnitOfWork : IDisposable
{
    IPollService PollService { get; }
    IAuthService AuthService { get; }
}
