using ErrorOr;
using GymManagement.Application.Authentication.Common;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUsersRepository usersRepository)
        : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByEmailAsync(query.Email);

        return user is null || !user.IsCorrectPasswordHash(query.Password, passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(user, jwtTokenGenerator.GenerateToken(user));
    }
}