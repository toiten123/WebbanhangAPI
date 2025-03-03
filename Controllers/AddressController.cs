using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data;
using QuanLyBanHang.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // API để lưu địa chỉ người dùng
        [HttpPost("SaveAddress")]
        public async Task<IActionResult> SaveAddress([FromBody] Address addressInput)
        {
            if (addressInput == null || addressInput.UserID <= 0 || string.IsNullOrWhiteSpace(addressInput.CityName) || string.IsNullOrWhiteSpace(addressInput.DistrictName) || string.IsNullOrWhiteSpace(addressInput.WardName) || string.IsNullOrWhiteSpace(addressInput.AddressDetail))
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            // Kiểm tra xem người dùng có tồn tại không
            var user = _context.Users.FirstOrDefault(u => u.UserID == addressInput.UserID);
            if (user == null)
            {
                return NotFound(new { Message = "Người dùng không tồn tại." });
            }

            // Thêm địa chỉ vào cơ sở dữ liệu
            _context.Addresses.Add(addressInput);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Địa chỉ đã được lưu thành công." });
        }

        // API để lấy thông tin địa chỉ của người dùng
        [HttpGet("GetUserAddress/{userId}")]
        public IActionResult GetUserAddress(int userId)
        {
            var addresses = _context.Addresses.Where(a => a.UserID == userId).ToList();
            if (addresses.Count == 0)
            {
                return NotFound(new { Message = "Không tìm thấy địa chỉ cho người dùng này." });
            }

            return Ok(addresses);
        }
    }
}
