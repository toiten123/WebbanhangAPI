using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    [Table("SidebarMenu")]
    public class SidebarItem
    {
        public int Id               { get; set; }
        public string Title         { get; set; } = string.Empty;
        public string Icon          { get; set; } = string.Empty;
        public string Controller    { get; set; } = string.Empty;
        public int Order            { get; set; } 
        public string Action        { get; set; } = string.Empty;
        public bool IsActive        { get; set; } = true;
    }
}
