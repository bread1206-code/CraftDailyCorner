using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.CraftDailyCorner.Services.PostCommentReport;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers.Front
{
    public class PostController : Controller
    {
        private readonly ICreatorPostService _postService;
        private readonly IPostCommentReportService _postCommentReportService;

        public PostController(ICreatorPostService postService, IPostCommentReportService postCommentReportService)
        {
            _postService = postService;
            _postCommentReportService = postCommentReportService;
        }

        // 前台列表
        public async Task<IActionResult> Index(string? keyword, int page = 1)
        {
            var query = new VMPostIndexQuery
            {
                Keyword = keyword,
                Page = page,
                PageSize = 16
            };

            var vm = await _postService.GetPostIndexAsync(query);

            return View(vm);
        }

        // 前台單篇
        public async Task<IActionResult> Detail(string id)
        {
            var memberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var canView = await _postService
                .CanViewPostAsync(id, memberId);

            if (!canView)
                return Forbid();

            var post = await _postService
                .GetPostDetailAsync(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        // 後台列表
        [Authorize(Roles = "02")]
        public async Task<IActionResult> List()
        {
            var creatorId = User.GetCreatorId();
            var posts = await _postService
                .GetCreatorPostsAsync(creatorId);

            return View(posts);
        }

        // 建立
        [Authorize(Roles = "02")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMCreatorPostCreate vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _postService.CreateAsync(
                new CreateCreatorPostDTO
                {
                    Title = vm.Title,
                    Content = vm.Content,
                    Visibility = vm.Visibility,
                    ImageFile = vm.ImageFile
                },
                User.GetCreatorId()
            );

            return RedirectToAction(nameof(List));
        }

        // 編輯
        [Authorize(Roles = "02")]
        public async Task<IActionResult> Edit(string id)
        {
            var vm = await _postService
                .GetEditDataAsync(id, User.GetCreatorId());

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorPostEdit vm)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("=== ModelState Invalid ===");

                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"欄位: {state.Key}");
                        Console.WriteLine($"錯誤: {error.ErrorMessage}");
                    }
                }

                return View(vm);
            }

            await _postService.UpdateAsync(
                new UpdateCreatorPostDTO
                {
                    PostID = vm.PostID,
                    Title = vm.Title,
                    Content = vm.Content,
                    Visibility = vm.Visibility,
                    NewImageFile = vm.NewImageFile
                },
                User.GetCreatorId()
            );

            return RedirectToAction(nameof(List));
        }

        // 軟刪除
        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _postService
                .SoftDeleteAsync(id, User.GetCreatorId());

            return RedirectToAction(nameof(List));
        }
        //檢舉
        [HttpPost]
        [Authorize(Roles = "02")] // 創作者
        [ValidateAntiForgeryToken]
        public IActionResult ReportComment(string commentId, string reason)
        {
            var memberId = User.GetMemberId();

            var response = _postCommentReportService
                .CreateReport(commentId, memberId, reason);

            return response.Result switch
            {
                ReportCommentResult.Success =>
                    RedirectToAction("Detail", new { id = response.PostId }),

                ReportCommentResult.NotFound =>
                    NotFound(),

                ReportCommentResult.Forbidden =>
                    Forbid(),

                ReportCommentResult.AlreadyReported =>
                    RedirectToAction("Detail", new { id = response.PostId }),

                _ => BadRequest()
            };
        }
    }
}