using ErrorOr;
using GymManagement.Application.Authentication.Commands.Register;
using GymManagement.Application.Authentication.Common;
using GymManagement.Application.Authentication.Queries.Login;
using GymManagement.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[Microsoft.AspNetCore.Components.Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController(ISender mediator) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
        ErrorOr<AuthenticationResult> authResult = await mediator.Send(command);

        return authResult.Match(
            result => base.Ok(MapToAuthResponse(result)),
            Problem);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == AuthenticationErrors.InvalidCredentials)
        {
            return Problem(
                detail: authResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return authResult.Match(
            result => Ok(MapToAuthResponse(result)),
            Problem);
    }
    
    private static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);
    }
}