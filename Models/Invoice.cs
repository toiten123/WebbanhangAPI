using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required, ForeignKey("Orders")]
        public int OrdersID { get; set; }

        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public Orders Orders { get; set; }
    }
}
