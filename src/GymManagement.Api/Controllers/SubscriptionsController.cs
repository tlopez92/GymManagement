using GymManagement.Application.Services;
using GymManagement.Contracts.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService _subscriptionServices;

    public SubscriptionsController(ISubscriptionsService subscriptionServices)
    {
        _subscriptionServices = subscriptionServices;
    }
    
    [HttpPost]
    public IActionResult CreateSubscription(CreateSubscriptionRequest request)
    {
        var subscriptionId = _subscriptionServices.CreateSubscription(request.SubscriptionType.ToString(), request.AdminId);
        var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);
        return Ok(response);
    }
}