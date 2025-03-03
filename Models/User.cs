using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuanLyBanHang.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;

        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Gender { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        // Khởi tạo danh sách rỗng để tránh lỗi Validation
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; } = new List<CustomerVoucher>();

        [JsonIgnore] // Ngăn lỗi serialization khi gọi API
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}
