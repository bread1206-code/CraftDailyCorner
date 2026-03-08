using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Filters
{
    public class SuspendedMemberGuardFilter : IAsyncActionFilter
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IAuthService _authService;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        private const byte MEMBER_SUSPENDED = 2;

        public SuspendedMemberGuardFilter(
            CraftDailyCornerContext context,
            IAuthService authService,
            ITempDataDictionaryFactory tempDataFactory)
        {
            _context = context;
            _authService = authService;
            _tempDataFactory = tempDataFactory;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            // 1. 未登入：不用檢查
            if (user?.Identity?.IsAuthenticated != true)
            {
                await next();
                return;
            }

            // 2. AllowAnonymous：直接放行
            if (HasAllowAnonymous(context))
            {
                await next();
                return;
            }

            // 3. 找不到 MemberID：直接登出並導登入頁
            var memberId = user.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
            {
                await ForceLogoutAndRedirectToLogin(context, "登入狀態異常，請重新登入");
                return;
            }

            // 4. 查會員狀態
            var member = await _context.Members
                .AsNoTracking()
                .Where(m => m.MemberID == memberId)
                .Select(m => new
                {
                    m.MemberID,
                    m.StatusID
                })
                .FirstOrDefaultAsync();

            // 5. 會員不存在 or 已停權 => 強制登出
            if (member == null)
            {
                await ForceLogoutAndRedirectToLogin(context, "找不到會員資料，請重新登入");
                return;
            }

            if (member.StatusID == MEMBER_SUSPENDED)
            {
                await ForceLogoutAndRedirectToLogin(context, "此帳號已被停權，請聯絡管理員");
                return;
            }

            await next();
        }

        private static bool HasAllowAnonymous(ActionExecutingContext context)
        {
            // Endpoint metadata
            if (context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return true;

            // Controller / Action attributes
            if (context.ActionDescriptor is ControllerActionDescriptor cad)
            {
                bool controllerAllowAnonymous = cad.ControllerTypeInfo
                    .GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                    .Any();

                bool actionAllowAnonymous = cad.MethodInfo
                    .GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                    .Any();

                return controllerAllowAnonymous || actionAllowAnonymous;
            }

            return false;
        }

        private async Task ForceLogoutAndRedirectToLogin(
            ActionExecutingContext context,
            string message)
        {
            await _authService.SignOutAsync(context.HttpContext);

            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["Warning"] = message;

            context.Result = new RedirectToActionResult(
                actionName: "Login",
                controllerName: "Account",
                routeValues: null);
        }
    }
}