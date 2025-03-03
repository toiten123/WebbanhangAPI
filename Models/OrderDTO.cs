using QuanLyBanHang.Models;
using System.ComponentModel.DataAnnotations;
namespace QuanLyBanHang.Models
{
    public class OrderDTO
    {
        [Required]
        public int UserID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string OrdersName { get; set; }

        [Required]
        [Phone]
        public string OrdersPhone { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public string DistrictName { get; set; }

        [Required]
        public string WardName { get; set; }

        [Required]
        public string Address { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal ShippingFee { get; set; }

        [Required]
        public string PaymentTypeName { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        // Thêm danh sách các sản phẩm trong đơn hàng
        [Required]
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }

      public class OrderDetailDTO
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
    public class OrderStatusHistoryDTO
    {
        
        public string Status { get; set; }

        public string Notes { get; set; }

        
        public DateTime UpdatedDate { get; set; }
    }
}
