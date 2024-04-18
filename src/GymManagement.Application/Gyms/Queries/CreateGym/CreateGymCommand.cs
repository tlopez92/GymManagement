using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.CreateGym;

public record CreateGymCommand(string Name, Guid SubscriptionId): IRequest<ErrorOr<Gym>>;