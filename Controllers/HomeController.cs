using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebShopSK.Data;
using WebShopSK.Extensions;
using WebShopSK.Models;
using WebShopSK.ViewModels;

namespace WebShopSK.Controllers
{
    public class HomeController : Controller
    {
        public const string SessionKeyName = "_cart";

        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? message)
        {
            ViewBag.Message = message ?? "";

            return View();
        }

        public IActionResult Products(int? categoryId)
        {
            ViewBag.Categories = _context.Categories.ToList();

            List<Product> products = new List<Product>();

            if (categoryId == null)
            {
                products = _context.Products.ToList();
            }
            else
            {
                products = (
                                from product in _context.Products
                                join product_category in _context.ProductCategories 
                                    on product.Id equals product_category.ProductId
                                where product_category.CategoryId == categoryId
                                select new Product 
                                { 
                                    Id = product.Id,
                                    Title = product.Title,
                                    Description = product.Description,
                                    Quantity = product.Quantity,
                                    Price = product.Price
                                }
                            ).ToList();
                //products =  _context.ProductCategories
                //    .Where(pc => pc.CategoryId == categoryId)
                //    .SelectMany(pc => _context.Products.Where(p => p.Id == pc.ProductId))
                //    .ToListAsync();
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]


        public IActionResult Order()
        {
            List<CartItem> sessionCartItems =
                HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (sessionCartItems.Count == 0)
            {
                RedirectToAction(nameof(Index));
            }

            ViewBag.CartItems = sessionCartItems;

            decimal Total = 0;
            ViewBag.TotalPrice = sessionCartItems.Sum(item => Total + item.GetTotal());

            ViewBag.ShorRemoveAction = false;


			return View();
        }

        [HttpPost]
		public IActionResult CreateOrder(UserOrderViewModel orderViewModel)
		{

			List<CartItem> sessionCartItems =
				HttpContext.Session.GetObjectFromJSON<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

			if (sessionCartItems.Count == 0)
			{
				RedirectToAction(nameof(Index));
			}

            var order = new Order()
            {
                BillingAdress = orderViewModel.OrderAdress.BillingAdress,
                BillingCity = orderViewModel.OrderAdress.BillingCity,
                BillingZipCode = orderViewModel.OrderAdress.BillingZipCode,
                BillingEmail = orderViewModel.OrderAdress.BillingEmail,
                BillingFirstName = orderViewModel.OrderAdress.BillingFirstName,
                BillingLastName = orderViewModel.OrderAdress.BillingLastName,
                BillingPhone = orderViewModel.OrderAdress.BillingPhone,
                Message = orderViewModel.Message,
				Total = orderViewModel.TotalPrice
            };

            _context.Add(order);
            _context.SaveChanges();

            var orderId = order.Id;

            foreach (var item in sessionCartItems)
            {
                OrderItem orderItem = new OrderItem()
                {
                    OrderId = orderId,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Total = item.GetTotal()
                };

                _context.Add(orderItem);
                _context.SaveChanges();
			}

            HttpContext.Session.SetObjectAsJSON(SessionKeyName, "");

            return RedirectToAction("Index", new { message = "Thank xou for order number " + orderId + " :)" } );
		}

		public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
