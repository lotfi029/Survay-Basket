using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Survay_Basket.API.Health;

public class MailProviderHealthCheck(IOptions<MailSetting> mailSettings) : IHealthCheck
{
    private readonly MailSetting _mailSettings = mailSettings.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, port:  _mailSettings.Port, options: SecureSocketOptions.StartTls, cancellationToken: cancellationToken);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password, cancellationToken: cancellationToken);

            return await Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex) 
        {
            return await Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
