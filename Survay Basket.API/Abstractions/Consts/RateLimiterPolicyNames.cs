namespace Survay_Basket.API.Abstractions.Consts;

public class RateLimiterPolicyNames
{
    public const string Concurrency = "concurrency";
    public const string SlidingWindow = "sliding";
    public const string FixedWidnow = "fixed";
    public const string TokenBucket = "token";

    public const string IpLimiter = "ipLimit";
    public const string UserLimiter = "userLimit";
}
