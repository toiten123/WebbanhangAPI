using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Models;
using QuanLyBanHang.Data;

namespace QuanLyBanHang.Services
{
    public class SidebarService
    {
        private readonly ApplicationDbContext _context;

        public SidebarService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Lấy danh sách Sidebar theo thứ tự Order
        public async Task<List<SidebarItem>> GetSidebarItemsAsync()
        {
            var items = await _context.SidebarItems
                                    .Where(x => x.IsActive)
                                    .OrderBy(x => x.Order)
                                    .ToListAsync();

            if (items == null || !items.Any())
            {
                Console.WriteLine("⚠ SidebarItems trả về RỖNG!");
                return new List<SidebarItem>(); // Tránh null
            }

            return items;
        }

        // ✅ Thêm mới SidebarItem
        public async Task AddSidebarItemAsync(SidebarItem item)
        {
            _context.SidebarItems.Add(item);
            await _context.SaveChangesAsync();
        }

        // ✅ Cập nhật SidebarItem
        public async Task UpdateSidebarItemAsync(SidebarItem item)
        {
            var existingItem = await _context.SidebarItems.FindAsync(item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.Icon = item.Icon;
                existingItem.Controller = item.Controller;
                existingItem.Action = item.Action;
                existingItem.Order = item.Order;
                existingItem.IsActive = item.IsActive;

                await _context.SaveChangesAsync();
            }
        }

        // ✅ Xóa SidebarItem
        public async Task DeleteSidebarItemAsync(int id)
        {
            var item = await _context.SidebarItems.FindAsync(id);
            if (item != null)
            {
                _context.SidebarItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Cập nhật thứ tự Sidebar khi kéo & thả (Drag & Drop)
        public async Task UpdateOrderAsync(List<SidebarItem> items)
        {
            foreach (var item in items)
            {
                var dbItem = await _context.SidebarItems.FindAsync(item.Id);
                if (dbItem != null)
                {
                    dbItem.Order = item.Order;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
