using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRoomCommandHandler(
        IGymsRepository gymsRepository,
        IUnitOfWork unitOfWork)
    {
        _gymsRepository = gymsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(request.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }
        
        if (!gym.HasRoom(request.RoomId))
        {
            return Error.NotFound(description: "Room not found");
        }

        gym.RemoveRoom(request.RoomId);
        
        await _gymsRepository.UpdateGymAsync(gym);
        await _unitOfWork.CommitChangesAsync();
        
        return Result.Deleted;
    }
}