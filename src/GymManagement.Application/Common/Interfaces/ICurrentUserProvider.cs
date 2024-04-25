using GymManagement.Application.Models;

namespace GymManagement.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}