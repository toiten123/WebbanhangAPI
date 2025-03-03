using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class Orders
    {
        [Key]
        public int OrdersID { get; set; }  // Mã đơn hàng

        [Required]
        public int UserID { get; set; }  // Mã người dùng

        [Required]
        [StringLength(255)]  // Thêm StringLength cho các trường NVARCHAR(MAX)
        public string OrdersName { get; set; }  // Tên người đặt hàng

        [Required]
        [StringLength(255)]  // Thêm StringLength cho các trường NVARCHAR(MAX)
        public string OrdersPhone { get; set; }  // Số điện thoại người đặt hàng

        [Required]
        [StringLength(255)]  // Thêm StringLength cho các trường NVARCHAR(MAX)
        public string CityName { get; set; }  // Tên thành phố

        [Required]
        [StringLength(255)]  // Thêm StringLength cho các trường NVARCHAR(MAX)
        public string DistrictName { get; set; }  // Tên quận

        [Required]
        [StringLength(255)]  // Thêm StringLength cho các trường NVARCHAR(MAX)
        public string WardName { get; set; }  // Tên phường

        [Required]
        [StringLength(255)]  // Địa chỉ chi tiết
        public string Address { get; set; }

        [StringLength(1000)]  // Mô tả đơn hàng, cho phép null
        public string Description { get; set; }

        [Required]
        public DateTime OrdersDate { get; set; } = DateTime.Now;  // Ngày đặt hàng

        public DateTime? ShippedDate { get; set; }  // Ngày giao hàng (nullable)

        [StringLength(100)]  // Tên người nhận hàng
        public string ShipName { get; set; }

        [Column(TypeName = "decimal(10,2)")]  // Phí vận chuyển
        public decimal ShippingFee { get; set; } = 0;

        [Required]
        [StringLength(50)]  // Loại thanh toán
        public string PaymentTypeName { get; set; }

        [StringLength(50)]  // Trạng thái đơn hàng
        public string OrderStatus { get; set; } = "Pending";

        [Required]
        [Column(TypeName = "decimal(19,4)")]  // Tổng giá trị đơn hàng
        public decimal TotalPrice { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;  // Ngày tạo đơn hàng

        // Quan hệ với các bảng khác
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }  // Quan hệ với OrdersDetails

        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }  // Quan hệ với OrderStatusHistory
    }
}
