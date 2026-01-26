using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CraftDailyCorner.Models;

namespace CraftDailyCorner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//在套件管理器主控台(檢視 > 其他視窗 > 套件管理器主控台)下指令
//      ※※※注意注意※※※ 在執行指令前請先確定專案是否正確選擇
//      ※※※注意注意※※※ 在執行指令前請先確定專案是否正確選擇
//      ※※※注意注意※※※ 在執行指令前請先確定專案是否正確選擇
//      (1)Add-Migration InitialCreate  (這個指令是產生建立資料庫的程式碼)
//      (2)Update-database    (這個指令是在資料庫中執行上一步所產生的程式碼)
//      ※第(1)項的「InitialCreate﹞是自訂的名稱，若執行成功會看到「Build succeeded.」※
//      ※另外會看到一個Migrations的資料夾及其檔案被建立在專案中，裡面紀錄著Migration的歷程※
//      ※第(1)項指令執行成功才能執行第(2)項指令※
//      (3)至SSMS中查看是否有成功建立資料庫及資料表(目前資料表內沒有資料)