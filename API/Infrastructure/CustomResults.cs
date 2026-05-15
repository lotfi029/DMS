using Microsoft.AspNetCore.Mvc;
using SharedKernel;

public static class CustomResults
{
    public static IResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannet convert a successful result to a problem.");

        var problem = Results.Problem(statusCode: result.Error.Status);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>()
        {
            {
                "errors", GetErrors(result)
            }
        };
        static Dictionary<string, List<string>> GetErrors(Result result)
        {
            if (result.Error is ValidationError validationError)
            {
                var r = new Dictionary<string, List<string>>();
                foreach(var error in validationError.Errors)
                {
                    if (r.TryGetValue(error.Code, out var values))
                        values.Add(error.Description);
                    else
                        r.Add(error.Code, [error.Description]);

                }
                return r;
            }
            return new()
            {
                { result.Error.Code, [result.Error.Description] }
            };
        }
        return TypedResults.Problem(problemDetails);
    }
}