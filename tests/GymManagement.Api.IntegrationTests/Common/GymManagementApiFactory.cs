using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GymManagement.Api.IntegrationTests.Common;

public class GymManagementApiFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private SqlServerTestDatabase _testDatabase = null!;
    
    public HttpClient HttpClient { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqlServerTestDatabase.CreateAndInitialize();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<GymManagementDbContext>>()
                .AddDbContext<GymManagementDbContext>((_, options) => options.UseSqlServer(_testDatabase.Connection));
        });
    }
    
    public Task InitializeAsync()
    {
        HttpClient = CreateClient();

        return Task.CompletedTask;
    }
    
    public new Task DisposeAsync()
    {
        _testDatabase.Dispose();

        return Task.CompletedTask;
    }
    
    public void ResetDatabase()
    {
        _testDatabase.ResetDatabase();
    }
}