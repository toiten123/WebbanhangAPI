using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
    public class CreditCard
    {
        [Key]
        public int CardID { get; set; }
        public int UserID { get; set; }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
