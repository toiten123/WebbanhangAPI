using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanLyBanHang.Models;
using QuanLyBanHang.Services;

namespace QuanLyBanHang.Controllers
{
    [Route("api/sidebar")]
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private readonly SidebarService _sidebarService;

        public SidebarController(SidebarService sidebarService)
        {
            _sidebarService = sidebarService;
        }

        // ✅ Lấy danh sách Sidebar
        [HttpGet]
        public async Task<ActionResult<List<SidebarItem>>> GetSidebarItems()
        {
            var sidebarItems = await _sidebarService.GetSidebarItemsAsync();
            return Ok(sidebarItems);
        }

        // ✅ Thêm mới Sidebar
        [HttpPost]
        public async Task<IActionResult> AddSidebarItem([FromBody] SidebarItem item)
        {
            await _sidebarService.AddSidebarItemAsync(item);
            return Ok(new { success = true, message = "Thêm thành công", item });
        }

        // ✅ Cập nhật Sidebar
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSidebarItem(int id, [FromBody] SidebarItem item)
        {
            if (item == null || id != item.Id)
                return BadRequest("ID không khớp hoặc dữ liệu không hợp lệ");

            await _sidebarService.UpdateSidebarItemAsync(item);
            return Ok(new { success = true, message = "Cập nhật thành công" });
        }

        // ✅ Xóa Sidebar
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSidebarItem(int id)
        {
            await _sidebarService.DeleteSidebarItemAsync(id);
            return Ok(new { success = true, message = "Xóa thành công" });
        }

        // ✅ Cập nhật thứ tự Sidebar (Drag & Drop)
        [HttpPost("update-order")]
        public async Task<IActionResult> UpdateOrder([FromBody] List<SidebarItem> items)
        {
            await _sidebarService.UpdateOrderAsync(items);
            return Ok(new { success = true, message = "Cập nhật thứ tự thành công" });
        }
    }
}
