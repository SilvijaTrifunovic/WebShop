using System.ComponentModel.DataAnnotations;

namespace WebShopSK.ViewModels
{
    public class OrderAdress
    {
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
    }
}
