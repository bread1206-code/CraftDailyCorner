using CraftDailyCorner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/favorite")]
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("toggle")]
        public IActionResult Toggle([FromForm] string productId)
        {
            try
            {
                var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(memberId))
                {
                    return Unauthorized("memberId is null");
                }

                if (string.IsNullOrEmpty(productId))
                {
                    return BadRequest("productId is null");
                }

                if (_favoriteService.IsFavorite(memberId, productId))
                {
                    _favoriteService.RemoveFavorite(memberId, productId);
                    return Ok(new { isFavorite = false });
                }

                _favoriteService.AddFavorite(memberId, productId);
                return Ok(new { isFavorite = true });
            }
            catch (Exception ex)
            {
                // 🔥 關鍵：把錯誤直接吐出來
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}
