using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Users;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Users.Persistence;

public class UsersRepository(GymManagementDbContext dbContext) : IUsersRepository
{
    public async Task AddUserAsync(User user)
    {
        await dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    public Task UpdateAsync(User user)
    {
        dbContext.Update(user);
        return Task.CompletedTask;
    }
}