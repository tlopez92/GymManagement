using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public SubscriptionsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<bool> ExistsAsync(Guid subscriptionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Subscription?> GetByAdminIdAsync(Guid adminId)
    {
        throw new NotImplementedException();
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await _dbContext.Subscriptions.FindAsync(subscriptionId);
    }

    public async Task<List<Subscription>> ListAsync()
    {
        throw new NotImplementedException();
    }

    public async Task RemoveSubscriptionAsync(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        throw new NotImplementedException();
    }
}