using System.Security.Claims;

namespace LojaTobias.Core.Interfaces
{
    public interface IAspnetUser
    {
        string Name { get; }
        string Action { get; }
        string GetUserId();
        string GetUserRole();
        string GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
