using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSK.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(9, 2)")] // to je 9 brojeva od cega je 7 brojeva i 2 decimale
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Total { get; set; }
        [NotMapped]
        public string ProductTitle { get; set; }
    }
}
