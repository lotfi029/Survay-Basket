using Azure;
using System.Diagnostics;

namespace Survay_Basket.API.Abstractions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result, int statusCode)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");


        var problem = Results.Problem(statusCode: statusCode);

        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>()
        {
            {
                "Errors",  new[]{result.Error}
            }
        };

        return new ObjectResult(problemDetails);
    }
}
