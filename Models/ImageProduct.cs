using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
public class ImageProduct
    {
        [Key]
        public int ImageProductID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public string ImageThumbnail { get; set; } // URL ảnh thu nhỏ

        [Required]
        public string ImageData { get; set; } // URL nhiều ảnh sản phẩm

        public virtual Product Product { get; set; }
    }

}
