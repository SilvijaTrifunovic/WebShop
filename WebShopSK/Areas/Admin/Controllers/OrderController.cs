using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopSK.Data;
using WebShopSK.Models;
using WebShopSK.ViewModels;

namespace WebShopSK.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderItems = (from orderitem in _context.OrderItems
                                where orderitem.OrderId == order.Id
                                select new OrderItem()
                                    {
                                        Id = orderitem.Id,
                                        OrderId = orderitem.OrderId,
                                        ProductId = orderitem.ProductId,
                                        Quantity = orderitem.Quantity,
                                        Total = orderitem.Total,
                                        ProductTitle = (from product in _context.Products
                                                         where product.Id == orderitem.ProductId
                                                         select product.Title
                                                         ).FirstOrDefault()
                                }
                                ).ToList();

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.Select(user => new SelectListItem
                    {
                        Value = user.Id.ToString(),
                        Text = user.FirstName + " " + user.LastName
                    }
                ).ToList();

            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            try
            {
                var order = new Order()
                {
                    Message = orderViewModel.Message,
                    UserId = orderViewModel.UserId,
                    BillingAdress = orderViewModel.OrderAdress.BillingAdress,
                    BillingCity = orderViewModel.OrderAdress.BillingCity,
                    BillingEmail = orderViewModel.OrderAdress.BillingEmail,
                    BillingFirstName = orderViewModel.OrderAdress.BillingFirstName,
                    BillingLastName = orderViewModel.OrderAdress.BillingLastName,
                    BillingPhone = orderViewModel.OrderAdress.BillingPhone,
                    BillingZipCode = orderViewModel.OrderAdress.BillingZipCode
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = order.Id});
            }
            catch (Exception ex)
            {
                return View(orderViewModel);
            }
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            var orderViewModel = new OrderViewModel()
            {
                Id = order.Id,
                Total = order.Total,
                Message = order.Message,
                UserId = order.UserId,
                DateCreated = order.DateCreated,
                OrderAdress = new OrderAdress()
                {
                    BillingAdress = order.BillingAdress,
                    BillingCity = order.BillingCity,
                    BillingEmail = order.BillingEmail,
                    BillingFirstName = order.BillingFirstName,
                    BillingLastName = order.BillingLastName,
                    BillingPhone = order.BillingPhone,
                    BillingZipCode = order.BillingZipCode
                }
            };

            var userOrder = await _context.Users.FirstOrDefaultAsync(user => user.Id == order.UserId );

            ViewBag.User = userOrder.FirstName + " " + userOrder.LastName;

            if (order == null)
            {
                return NotFound();
            }
            return View(orderViewModel);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            Order order = new Order();

            try
            {
                order = _context.Orders.FirstOrDefault(order => order.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                order.BillingAdress = orderViewModel.OrderAdress.BillingAdress;
                order.BillingCity = orderViewModel.OrderAdress.BillingCity;
                order.BillingZipCode = orderViewModel.OrderAdress.BillingZipCode;
                order.BillingEmail = orderViewModel.OrderAdress.BillingEmail;
                order.BillingFirstName = orderViewModel.OrderAdress.BillingFirstName;
                order.BillingLastName = orderViewModel.OrderAdress.BillingLastName;
                order.BillingPhone = orderViewModel.OrderAdress.BillingPhone;

                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound();
                }
                else
                {
                    return View(order);
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
