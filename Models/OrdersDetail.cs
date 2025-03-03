using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class OrdersDetail
    {
        [Key]
        public int OrderDetailID { get; set; }  // Mã chi tiết đơn hàng

        [Required]
        public int OrdersID { get; set; }  // Mã đơn hàng

        [Required]
        public int ProductID { get; set; }  // Mã sản phẩm

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")] 
        public int Quantity { get; set; }  // Số lượng sản phẩm

        // Thiếu trường `UnitPrice` và `DiscountPercentage` so với model cũ,
        // nhưng SQL không có hai cột này, nên ta sẽ xóa chúng.
        
        // Khai báo quan hệ với Orders và Product
        [ForeignKey("OrdersID")]
        public virtual Orders Orders { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
