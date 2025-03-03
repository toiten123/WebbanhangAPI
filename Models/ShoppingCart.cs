using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ShoppingCartDetails> ShoppingCartDetails { get; set; }
    }
}
