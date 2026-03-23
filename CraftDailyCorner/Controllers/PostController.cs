using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    public class PostController : Controller
    {
        private readonly ICreatorPostService _postService;

        public PostController(ICreatorPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Test()
        {
            return Content("OK");
        }

        // 前台列表
        public async Task<IActionResult> Index(string? PostKeyword, int page = 1)
        {
            var query = new VMPostIndexQuery
            {
                PostKeyword = PostKeyword,
                Page = page,
                PageSize = 9
            };

            var currentMemberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var vm = await _postService.GetPostIndexAsync(query, currentMemberId);

            return View(vm);
        }

        // 前台單篇
        public async Task<IActionResult> Detail(string id)
        {
            var currentMemberId = User.Identity?.IsAuthenticated == true
                ? User.GetMemberId()
                : null;

            var canView = await _postService.CanViewPostAsync(id, currentMemberId);

            if (!canView)
                return Forbid();

            var post = await _postService.GetPostDetailAsync(id, currentMemberId);

            if (post == null)
                return NotFound();

            return View(post);
        }

        // 後台列表
        [Authorize(Roles = "02")]
        public async Task<IActionResult> List()
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var posts = await _postService.GetCreatorPostsAsync(creatorId);

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
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

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
                creatorId
            );

            return RedirectToAction(nameof(List));
        }

        // 編輯
        [Authorize(Roles = "02")]
        public async Task<IActionResult> Edit(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            var vm = await _postService.GetEditDataAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMCreatorPostEdit vm)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

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
                creatorId
            );

            return RedirectToAction(nameof(List));
        }

        // 軟刪除
        [HttpPost]
        [Authorize(Roles = "02")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var creatorId = User.GetCreatorId();
            if (string.IsNullOrWhiteSpace(creatorId))
                return Unauthorized();

            await _postService.SoftDeleteAsync(id, creatorId);

            return RedirectToAction(nameof(List));
        }
    }
}