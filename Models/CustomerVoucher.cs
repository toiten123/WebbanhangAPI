using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
    public class CustomerVoucher
    {
        [Key]
        public int CustomerVoucherID { get; set; }
        public int UserID { get; set; }
        public int VoucherID { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Created { get; set; }

        public virtual User User { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
