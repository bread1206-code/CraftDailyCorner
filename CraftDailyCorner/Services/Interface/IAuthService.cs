using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CraftDailyCorner.Services.Interface
{
    public interface IAuthService
    {
        Task SignInMemberAsync(HttpContext httpContext, string memberId);
        Task RefreshSignInAsync(HttpContext httpContext, string memberId);
        Task SignOutAsync(HttpContext httpContext);
    }
}