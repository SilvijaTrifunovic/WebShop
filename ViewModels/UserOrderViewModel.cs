using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebShopSK.ViewModels
{
    public class UserOrderViewModel
	{
        public OrderAdress OrderAdress { get; set; }
		public string Message { get; set; }

		[Column(TypeName = "decimal(9,2)")]
		[Required(ErrorMessage = "Total is required")]
		public decimal TotalPrice { get; set; }
	}
}
