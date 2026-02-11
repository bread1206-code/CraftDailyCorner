using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPost;
using CraftDailyCorner.ViewModels.CreatorPost.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers.Front
{
    public class PostController : Controller
    {
        private readonly ICreatorPostService _postService;
        private readonly IImageUploadService _imageUploadService;

        public PostController(
            ICreatorPostService postService,
            IImageUploadService imageUploadService)
        {
            _postService = postService;
            _imageUploadService = imageUploadService;
        }

        //前台列表（公開）

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

        //前台單篇

        public async Task<IActionResult> Detail(string id)
        {
            var post = await _postService.GetPublicPostDetailAsync(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        //後台列表

        [Authorize(Roles = "02")]
        public async Task<IActionResult> List()
        {
            var creatorId = User.GetCreatorId();
            var posts = await _postService.GetCreatorPostsAsync(creatorId);

            return View(posts);
        }

        //建立

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

            var imageKey = _imageUploadService.UploadImage(
                vm.ImageFile,
                null,
                "05CreatorPost",
                ImageSizePresets.Post
            );

            await _postService.CreateAsync(
                new CreateCreatorPostDTO
                {
                    Title = vm.Title,
                    Content = vm.Content,
                    ImageUrl = imageKey,
                    Visibility = vm.Visibility
                },
                User.GetCreatorId()
            );

            return RedirectToAction(nameof(List));
        }

        //編輯

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
                return View(vm);

            string imageKey = vm.CurrentImageUrl;

            if (vm.NewImageFile != null)
            {
                imageKey = _imageUploadService.UploadImage(
                    vm.NewImageFile,
                    null,
                    "Post",
                    ImageSizePresets.Post,
                    vm.PostID
                );
            }

            await _postService.UpdateAsync(
                new UpdateCreatorPostDTO
                {
                    PostID = vm.PostID,
                    Title = vm.Title,
                    Content = vm.Content,
                    ImageUrl = imageKey,
                    Visibility = vm.Visibility
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
    }
}