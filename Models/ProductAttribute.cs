using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuanLyBanHang.Models
{
    public class ProductAttribute
    {
        [Key]
        public int AttributeID { get; set; } // ID thuộc tính sản phẩm, tự tăng

        [ForeignKey("Product")]
        public int ProductID { get; set; } // Liên kết với bảng Product

        [Required]
        [StringLength(255)]
        public string ProductAttributeName { get; set; } // Tên nhóm thông số (VD: Mainboard, CPU, RAM,...)

        [Required]
        [StringLength(255)]
        public string AttributeName { get; set; } // Tên thuộc tính cụ thể (VD: Tốc độ CPU, Dung lượng RAM,...)

        [StringLength(255)]
        public string? AttributeSummary { get; set; } // Tên tóm tắt sản phẩm, có thể NULL


        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
}
