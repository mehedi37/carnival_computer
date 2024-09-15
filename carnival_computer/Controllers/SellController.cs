using carnival_computer.Areas.Identity.Data;
using carnival_computer.Data;
using carnival_computer.Models;
using carnival_computer.Services;
using carnival_computer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace carnival_computer.Controllers
{
    [Authorize]
    public class SellController : Controller
    {
        private readonly ILogger<SellController> _logger;
        private readonly carnival_computerContext _context;
        private readonly UserManager<carnival_computerUser> _userManager;
        private readonly ProductServices _productService;
        private readonly CustomerServices _customerService;

        public SellController(ILogger<SellController> logger, carnival_computerContext context, UserManager<carnival_computerUser> userManager, ProductServices productService, CustomerServices customerService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IActionResult> SellComputer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var itemsForSale = await _productService.GetItemsForSaleByUserIdAsync(userId);
            var customers = await _customerService.GetCustomersBySellerIdAsync(userId);

            var model = new SellerViewModel
            {
                ItemsForSale = itemsForSale,
                Customers = customers
            };

            return View(model);
        }

        public IActionResult AddComputer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new Products
            {
                ProductId = new int(),
                ProductName = string.Empty,
                ProductDescription = string.Empty,
                ProductImage = string.Empty,
                ProductPrice = 0.0M,
                UserId = userId,
                Stock = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddComputer(Products product)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.UserId = userId ?? throw new InvalidOperationException("User ID cannot be null");
                await _productService.AddItemAsync(product);
                return RedirectToAction("SellComputer");
            }
            return View(product);
        }

        public async Task<IActionResult> EditComputer(int id)
        {
            var product = await _productService.GetItemByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditComputer(Products product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateItemAsync(product);
                return RedirectToAction("SellComputer");
            }
            return View(product);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var product = await _productService.GetItemByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (product.UserId != userId)
            {
                return Unauthorized();
            }

            await _productService.DeleteItemAsync(id);
            return RedirectToAction("SellComputer");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
