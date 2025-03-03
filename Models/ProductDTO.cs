using QuanLyBanHang.Models;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
    public class ProductDTO
    {
        public Product Product { get; set; }

        public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

        [Required]
        public List<string> ImageData { get; set; } = new List<string>(); //Lưu ảnh qua URL cloudinary

        public string? ImageThumbnail { get; set; } // 🔹 Thêm trường ảnh Thumbnail qua URL cloudinary

    }
}
