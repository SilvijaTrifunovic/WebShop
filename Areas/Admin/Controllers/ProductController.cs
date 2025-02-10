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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.ProductCategories).ToListAsync();

            foreach (var product in products)
            {
                foreach (var productCategories in product.ProductCategories)
                {
                    var categDetails = _context.Categories.FirstOrDefault(c => c.Id == productCategories.CategoryId);

                    if (categDetails != null)
                    {
                        productCategories.CategoryTitle = categDetails.Title;
                    }
                }
            }

            return View(products);
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Quantity,Price")] Product product)
        {
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("AssignCategoryToProduct", "ProductCategory", new { productId = product.Id });
            }
            catch (Exception ex)
            {
                return View(product);
            }
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(c => c.ProductId == id);
            var categoryId = productCategory == null ? "0" : productCategory.CategoryId.ToString();


            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                CategoryId = (from pc in _context.ProductCategories
                              where pc.ProductId == product.Id
                              select pc.CategoryId).FirstOrDefault(),
            };

            var defaultSelect = new SelectListItem()
            {
                Value = "0",
                Text = "--- Select categori ---",
                Disabled = true,
                //Selected = categoryId == null
            };

            var categories = _context.Categories.Select(cat =>
                            new SelectListItem
                            {
                                Value = cat.Id.ToString(),
                                Text = cat.Title,
                                //Selected = cat.Id == categoryId
                            }
                            ).ToList();

            categories.Add(defaultSelect);

            ViewBag.Categiries = categories;


            return View(productViewModel);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

            try
            {
                var product = await _context.Products.SingleAsync(p => p.Id == productViewModel.Id);

                product.Title = productViewModel.Title;
                product.Description = productViewModel.Description;
                product.Quantity = productViewModel.Quantity;
                product.Price = productViewModel.Price;

                _context.Update(product);

                var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(c => c.ProductId == id);
                if (productCategory == null)
                {
                    productCategory = new ProductCategory()
                    {
                        ProductId = productViewModel.Id,
                        CategoryId = productViewModel.CategoryId,
                    };

                    _context.Add(productCategory);
                }
                else
                {
                    productCategory.CategoryId = productViewModel.CategoryId;

                    _context.Update(productCategory);
                }


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //dz logiranje
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
