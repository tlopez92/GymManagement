namespace GymManagement.Application.Models;

public record CurrentUser(Guid Id, IReadOnlyList<string> Permissions, IReadOnlyList<string> Roles);