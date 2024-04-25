using FluentValidation;
using GymManagement.Application.Gyms.Commands.CreateGym;

namespace GymManagement.Application.Gyms.Queries.CreateGym;

public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(100);
    }
}