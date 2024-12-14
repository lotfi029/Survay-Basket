namespace Survay_Basket.API.Services;

public interface INotificationService
{
    Task SendNewPollsNotification(int? id = null, CancellationToken cancellationToken = default);
}
