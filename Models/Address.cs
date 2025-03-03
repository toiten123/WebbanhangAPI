using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class Address
    {
        [Key]
        public int AddressID { get; set; }  // Mã địa chỉ

        [Required]
        public int UserID { get; set; }  // Mã người dùng

        [Required]
        [StringLength(255)]
        public string CityName { get; set; }  // Thành phố

        [Required]
        [StringLength(255)]
        public string DistrictName { get; set; }  // Quận/Huyện

        [Required]
        [StringLength(255)]
        public string WardName { get; set; }  // Phường/Xã

         [Required]
        [StringLength(255)]
        [Column("Address")] // Giữ đúng tên cột trong SQL
        public string AddressDetail { get; set; }  // Địa chỉ chi tiết (tránh lỗi CS0542)

        public string Description { get; set; }  // Mô tả thêm (có thể null)

        // Khai báo quan hệ với User
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}