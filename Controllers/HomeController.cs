using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()        {return View();}
        public IActionResult Products()     {return View("~/Views/Home/Products/Products.cshtml");}
        public IActionResult Cart()         {return View("~/Views/Home/Cart/ViewShoppingCart.cshtml");}
        public IActionResult ProductDetail(){return View("~/Views/Home/Cart/ViewProductDetail.cshtml");}
        public IActionResult Banking()      {return View("~/Views/Home/Cart/ViewBanking.cshtml");}
        public IActionResult Complete()     {return View("~/Views/Home/Cart/ViewComplete.cshtml");}
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
