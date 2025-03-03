using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
    public class Voucher
    {
        [Key]
        public int VoucherID { get; set; }
        public string? VoucherCode { get; set; }
        public string? VoucherName { get; set; }
        public int Uses { get; set; }
        public string? TypeOfVoucher { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
