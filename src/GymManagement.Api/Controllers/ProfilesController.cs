using MediatR;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Application.Profiles.Queries.ListProfiles;
using GymManagement.Contracts.Profiles;
using GymManagement.Application.Profiles.Commands.CreateAdminProfile;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Api.Controllers;

[Route("users/{userId:guid}/profiles")]
public class ProfilesController(ISender mediator) : ApiController
{
    [HttpPost("admin")]
    [Authorize]
    public async Task<IActionResult> CreateAdminProfile(Guid userId)
    {
        var command = new CreateAdminProfileCommand(userId);

        var createProfileResult = await mediator.Send(command);

        return createProfileResult.Match(
            id => Ok(new ProfileResponse(id)),
            Problem);
    }
    
    [HttpGet]
    public async Task<IActionResult> ListProfiles(Guid userId)
    {
        var listProfilesQuery = new ListProfilesQuery(userId);

        var listProfilesResult = await mediator.Send(listProfilesQuery);

        return listProfilesResult.Match(
            profiles => Ok(new ListProfilesResponse(
                profiles.AdminId,
                profiles.ParticipantId,
                profiles.TrainerId)),
            Problem);
    }
}