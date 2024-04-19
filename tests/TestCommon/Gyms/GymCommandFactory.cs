using GymManagement.Application.Gyms.Queries.CreateGym;
using TestCommon.TestConstants;

namespace TestCommon.Gyms;

public static class GymCommandFactory
{
    public static CreateGymCommand CreateCreateGymCommand(string name = Constants.Gym.Name,
        Guid? subscriptionId = null)
    {
        return new CreateGymCommand(name, subscriptionId ?? Constants.Subscriptions.Id);
    }
}