using GymManagement.Application.Authentication.Common;
using MediatR;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Common.Interfaces;
using GymManagement.Domain.Users;

namespace GymManagement.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IUsersRepository _usersRepository,
    IUnitOfWork _unitOfWork)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistsByEmailAsync(request.Email))
        {
            return Error.Conflict(description: "User already exists");
        }
        
        var hashPasswordResult = _passwordHasher.HashPassword(request.Password);
        
        if (hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }
        
        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            hashPasswordResult.Value);
        
        await _usersRepository.AddUserAsync(user);
        await _unitOfWork.CommitChangesAsync();
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}