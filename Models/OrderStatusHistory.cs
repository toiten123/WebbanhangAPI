using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class OrderStatusHistory
    {
        [Key]
        public int StatusHistoryID { get; set; }  // Mã lịch sử trạng thái đơn hàng

        [Required]
        public int OrdersID { get; set; }  // Mã đơn hàng

        [Required]
        [StringLength(50)]
        public string Status { get; set; }  // Trạng thái đơn hàng

        [StringLength(255)]
        public string? Notes { get; set; }  // Ghi chú (có thể null)

        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;  // Ngày cập nhật

        // Khai báo quan hệ với Orders
        [ForeignKey("OrdersID")]
        public virtual Orders Orders { get; set; }
    }
}
