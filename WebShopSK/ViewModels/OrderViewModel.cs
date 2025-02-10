using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebShopSK.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        [Required(ErrorMessage = "Total is required")]
        public decimal Total { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public OrderAdress OrderAdress { get; set; } 
    }
}
