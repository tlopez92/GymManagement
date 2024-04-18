using ErrorOr;
using FluentValidation;
using GymManagement.Application.Gyms.Queries.CreateGym;
using MediatR;

namespace GymManagement.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid)
        {
            return await next();
        }
        
        // convert errors to ErrorOr errors
        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));

        return (dynamic)errors;
    }
}