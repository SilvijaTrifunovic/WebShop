using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopSK.Models
{
    public class Order
    {
        public int Id { get; set; }
        //[DataType(DataType.Date)] //ako zelimo samo datum u bazi
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        [Required(ErrorMessage = "Total is required")]
        public decimal Total { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "First Name is required")]
        public string BillingFirstName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Last Name is required")]
        public string BillingLastName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "E-mail is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string BillingEmail { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Phone is required")]
        public string BillingPhone { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Adress is required")]
        public string BillingAdress { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "City is required")]
        public string BillingCity { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Postal is required")]
        public string BillingZipCode { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }
        [ForeignKey("OrderId")]
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
