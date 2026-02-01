using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Mvc;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.ViewModels;


namespace CraftDailyCorner.ViewComponents
{

public class VCCartModal : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session
                .GetObjectFromJson<List<VMCartItem>>("CART")
                ?? new List<VMCartItem>();

            var vm = new VMCartPage
            {
                Items = cart
            };

            return View(vm);
        }
    }



}
