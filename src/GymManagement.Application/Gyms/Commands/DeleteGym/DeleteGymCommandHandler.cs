using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.DeleteGym;

public class DeleteGymCommandHandler : IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository _gymRepository;
    private readonly ISubscriptionsRepository _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteGymCommandHandler(
        IGymsRepository gymRepository,
        ISubscriptionsRepository subscriptionRepository,
        IUnitOfWork unitOfWork)
    {
        _gymRepository = gymRepository;
        _subscriptionRepository = subscriptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand command, CancellationToken cancellationToken)
    {
        var gym = await _gymRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }
        
        var subscription = await _subscriptionRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        if (!subscription.HasGym(command.GymId))
        {
            return Error.Unexpected(description: "Gym not found");
        }
        
        subscription.RemoveGym(command.GymId);
        await _subscriptionRepository.UpdateAsync(subscription);
        await _gymRepository.RemoveGymAsync(gym);
        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}