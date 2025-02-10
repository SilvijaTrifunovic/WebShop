using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using WebShopSK.Data;
using WebShopSK.Extensions;
using WebShopSK.Models;
using WebShopSK.ViewModels;

namespace WebShopSK.Controllers
{
	public class CartController : Controller
	{
		public const string SessionKeyName = "_cart";
 
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}
		
		public IActionResult Index()
		{
			List<CartItem> sessionCartItems =
				HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

			ViewBag.CartItems = sessionCartItems;

			decimal Total = 0;
			ViewBag.TotalPrice = sessionCartItems.Sum(item => Total + item.GetTotal() );
			ViewBag.ShorRemoveAction = true;

			return View();
		}

		public IActionResult AddToCart(int productId, int selectedQuantity)
		{
			List<CartItem> sessionCartItems =
				HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

			if (sessionCartItems.Count == 0)
			{
				CartItem cartItem = new CartItem
				{
					Id = sessionCartItems.Count + 1,
                    Product = _context.Products.Find(productId),
					Quantity = selectedQuantity
				};

				sessionCartItems.Add(cartItem);
			}
			else 
			{
				int productIndex = IsExistingInCart(productId);

				if (productIndex == -1)
				{
					CartItem cartItem = new CartItem
					{
                        Id = sessionCartItems.Count + 1,
                        Product = _context.Products.Find(productId),
						Quantity = selectedQuantity
					};

					sessionCartItems.Add(cartItem);
				}
				else 
				{
					sessionCartItems[productIndex].IncressQuantity(selectedQuantity);
				}
			}

			HttpContext.Session.SetObjectAsJSON(SessionKeyName, sessionCartItems);

			return RedirectToAction("Products", "Home");
		}

		public IActionResult RemoveFromCart(int productId)
		{
			List<CartItem> sessionCartItems =
				HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

			int productIndex = IsExistingInCart(productId);

			if (productIndex == -1)
			{
				return NotFound();
			}
			else 
			{
				sessionCartItems.RemoveAt(productIndex);
            }

			HttpContext.Session.SetObjectAsJSON(SessionKeyName, sessionCartItems);
            
			return RedirectToAction(nameof(Index));
        }

        private int IsExistingInCart(int productId)
		{
			List<CartItem> cart = HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName);

			for (var i = 0; i < cart.Count; i++)
			{
				if (cart[i].Product.Id == productId)
				{
					return i;
				}
			}

			return -1;
        }
	}
}
