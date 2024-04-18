namespace GymManagement.Domain.Rooms;

public class Room
{
    public Guid Id { get; }
    public string Name { get; } = null!;
    public Guid GymdId { get; }
    public int MaxDailySessions { get; }

    public Room(
        string name,
        Guid gymId,
        int maxDailySessions,
        Guid? id = null)
    {
        Name = name;
        GymdId = gymId;
        MaxDailySessions = maxDailySessions;
        Id = id ?? Guid.NewGuid();
    }
}