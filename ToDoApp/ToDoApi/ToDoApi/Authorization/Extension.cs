using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ToDoApi.Authorization
{
    public static class UserExtension
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(s => s.Type == "https://to-do-app.com/username")?.Value;

        }
    }
}
