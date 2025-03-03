using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuanLyBanHang.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; } // ID sản phẩm

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm

        [Required]
        [StringLength(200)]
        public string Loai { get; set; } = string.Empty; // Loại sản phẩm (PC, Laptop,...)

        [StringLength(255)]
        public string? Manufacturer { get; set; } // Nhà sản xuất

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceOriginal { get; set; } // Giá gốc sản phẩm

        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentage { get; set; } // % Giảm giá (nullable)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAfterDiscount { get; set; } // Giá sau khi giảm

        [Required]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [StringLength(1000)]
        public string? Description { get; set; } // Mô tả sản phẩm

        public DateTime Created { get; set; } = DateTime.Now; // Ngày tạo

        [JsonIgnore]
        public virtual ICollection<ProductAttribute>? Attributes { get; set; } = new List<ProductAttribute>();

        [JsonIgnore]
        public virtual ICollection<ImageProduct>? Images { get; set; } = new List<ImageProduct>();

        [JsonIgnore]
        public virtual ICollection<OrdersDetail>? OrderDetails { get; set; } = new List<OrdersDetail>();

        [JsonIgnore]
        public virtual ICollection<ShoppingCartDetails>? CartDetails { get; set; } = new List<ShoppingCartDetails>();

    }
}
