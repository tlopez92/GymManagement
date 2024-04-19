using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.SubcutaneousTests.Common;

public class SqlServerTestDatabase : IDisposable
{
    public SqlConnection Connection { get; }

    public static SqlServerTestDatabase CreateAndInitialize()
    {
        var testDatabase = new SqlServerTestDatabase(
            @"Server=tcp:localhost,1433;
                            Initial Catalog=gym_management;
                            Persist Security Info=False;
                            User ID=sa;
                            Password=Password123;
                            MultipleActiveResultSets=False;
                            Encrypt=False;
                            TrustServerCertificate=False;
                            Connection Timeout=30;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        Connection.Open();
        var options = new DbContextOptionsBuilder<GymManagementDbContext>()
            .UseSqlServer(Connection)
            .Options;

        var context = new GymManagementDbContext(options, null!, null!);
        context.Database.EnsureCreated();
        
        foreach (var table in context.Model.GetEntityTypes())
        {
            var tableName = table.GetTableName();
            if (tableName != "Admins")
            {
                context.Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
            }
            context.Database.ExecuteSqlRaw($"UPDATE Admins SET SubscriptionId = null");
        }
        
        context.SaveChanges();
    }

    public void ResetDatabase()
    {
        Connection.Close();

        InitializeDatabase();
    }

    private SqlServerTestDatabase(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        Connection.Close();
    }
}