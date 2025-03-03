using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Models;
using System.Diagnostics;
using QuanLyBanHang.Services;

namespace QuanLyBanHang.Controllers
{
    public class AdminController: Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SidebarService _sidebarService;
        public AdminController(ILogger<AdminController> logger, SidebarService sidebarService)
        {
            _logger = logger;
            _sidebarService = sidebarService;
        }
        public IActionResult Index()        {return View();}
        public IActionResult Users()        {return View("~/Views/Admin/Users/ViewUser.cshtml");}
        public IActionResult AddUsers()     {return View("~/Views/Admin/Users/AddUser.cshtml");}
        public IActionResult EditUsers()    {return View("~/Views/Admin/Users/EditUser.cshtml");}
        public IActionResult Products()     {return View("~/Views/Admin/Products/ViewProducts.cshtml");}
        public IActionResult AddProducts()  {return View("~/Views/Admin/Products/AddProducts.cshtml");}
        public IActionResult EditProducts() {return View("~/Views/Admin/Products/EditProducts.cshtml");}
        public IActionResult Salary()       {return View("~/Views/Admin/Salary/ViewSalary.cshtml");}
        public IActionResult AddSalary()    {return View("~/Views/Admin/Salary/AddSalary.cshtml");}
        public IActionResult Profile()      {return View("~/Views/Admin/Profile/Profile.cshtml");}
        public IActionResult Payment()      {return View("~/Views/Admin/Payment/ViewPayment.cshtml");}
        public IActionResult AddPayment()   {return View("~/Views/Admin/Payment/AddPayment.cshtml");}
        public IActionResult Cloudinary()   {return View("~/Views/Admin/Cloudinary/Index.cshtml");}

        public async Task<IActionResult> Setting()
        {
            var sidebarItems = await _sidebarService.GetSidebarItemsAsync();
            return View("~/Views/Admin/Setting/Setting.cshtml", sidebarItems);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
