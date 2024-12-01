﻿using Azure;
using System.Diagnostics;

namespace Survay_Basket.API.Abstractions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");


        var problem = Results.Problem(statusCode: result.Error.statusCode);

        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>()
        {
            {
                "Errors",  new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };

        return new ObjectResult(problemDetails);
    }
}