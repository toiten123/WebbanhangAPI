using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required, ForeignKey("Orders")]
        public int OrdersID { get; set; }

        [Required]
        public string PaymentType { get; set; }

        public string PaymentStatus { get; set; } = "Pending";
        public decimal Amount { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Orders Orders { get; set; }
    }
}
