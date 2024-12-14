using Survay_Basket.API.Helpers;

namespace Survay_Basket.API.Services;

public class NotificationService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor,
    IEmailSender emailSender) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollsNotification(int? id = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<Poll> polls = [];

        if (id.HasValue)
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            polls = [poll!];
        }
        else
        {
            polls = await _context.Polls
                .Where(e => e.IsPublished && e.StartsAt.Date == DateTime.Today)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        
        var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Admin.Name);
        var origin = _contextAccessor.HttpContext!.Request.Headers.Origin;

        foreach(var poll in polls)
        {
            foreach(var user in users)
            {
                var placeholders = new Dictionary<string, string>
                {
                    {"{{name}}", user.FirstName },
                    {"{{pollTitle}}", poll.Title },
                    {"{{endDate}}", poll.EndsAt.ToString() },
                    {"{{url}}",  $"{origin}/polls/start/{poll.Id}"},
                };
                var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

                await _emailSender.SendEmailAsync(user.Email!, $"Survay Basket: New Poll - {poll.Title}", body);
            }
        }

    }
}