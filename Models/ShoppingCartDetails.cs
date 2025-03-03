using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class ShoppingCartDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartDetailID { get; set; }  // Mã chi tiết giỏ hàng

        [Required]
        public int UserID { get; set; }  // Người dùng

        [Required]
        public int ProductID { get; set; }  // Sản phẩm

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn 0.")]
        public int Quantity { get; set; }  // Số lượng sản phẩm

        public DateTime CreatedDate { get; set; } = DateTime.Now;  // Ngày thêm vào giỏ hàng

        // Khóa ngoại UserID liên kết với bảng User
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        // Khóa ngoại ProductID liên kết với bảng Product
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
