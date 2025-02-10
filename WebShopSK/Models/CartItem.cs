namespace WebShopSK.Models
{
	public class CartItem
	{
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

		public decimal GetTotal() 
		{
			return Product.Price * Quantity;
		}

		public void IncressQuantity(int quantity)
		{
			if (Quantity < 1)
			{
				return;
			}
			else
			{
				Quantity = Quantity + quantity;
			} 
		}
	}
}
