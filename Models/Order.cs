using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSK.Models
{
    public class Order
    {
        public Order()
        {
            DateCreated = DateTime.Now;
            Total = 0.00M;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; }

        [StringLength(50)]
        public string BillingFirstName { get; set; }

        [StringLength(50)]
        public string BillingLastName { get; set; }

        [StringLength(100)]
        public string BillingEmail { get; set; }

        [StringLength(100)]
        public string BillingPhone { get; set; }

        [StringLength(150)]
        public string BillingAdress { get; set; }

        [StringLength(50)]
        public string BillingCity { get; set; }

        [StringLength(20)]
        public string BillingZipCode { get; set; }

        public string? Message { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("OrderId")]
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
