using carnival_computer.Areas.Identity.Data;
using carnival_computer.Data;
using carnival_computer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace carnival_computer.Services
{
    public class CartServices
    {
        private readonly carnival_computerContext _context;
        private readonly UserManager<carnival_computerUser> _userManager;

        public CartServices(carnival_computerContext context, UserManager<carnival_computerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<CartDetails>> GetCartDetailsAsync(carnival_computerUser user)
        {
            var cart = await _context.Cart
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Products)
                .FirstOrDefaultAsync(c => c.UserId == user.Id && !c.IsPurchased);

            if (cart == null || cart.CartDetails == null)
            {
                return new List<CartDetails>();
            }

            return cart.CartDetails.ToList();
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Cart
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsPurchased);

            if (cart == null || cart.CartDetails == null)
            {
                return null;
            }
            return cart;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = new int(),
                    UserId = userId,
                    IsPurchased = false
                };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartDetails = await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.CartId == cart.CartId && cd.ProductId == productId);

            if (cartDetails == null)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                cartDetails = new CartDetails
                {
                    CartDetailsId = new int(),
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.ProductPrice
                };
                _context.CartDetails.Add(cartDetails);
            }
            else
            {
                cartDetails.Quantity += quantity;
                _context.CartDetails.Update(cartDetails);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(int cartDetailsId, int quantity)
        {
            var cartDetails = await _context.CartDetails.FindAsync(cartDetailsId);
            if (cartDetails != null)
            {
                cartDetails.Quantity = quantity;
                _context.CartDetails.Update(cartDetails);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(int cartDetailsId)
        {
            var cartDetails = await _context.CartDetails.FindAsync(cartDetailsId);
            if (cartDetails != null)
            {
                _context.CartDetails.Remove(cartDetails);
                await _context.SaveChangesAsync();

                var cart = await _context.Cart
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.CartId == cartDetails.CartId);

                if (cart != null && !cart.CartDetails.Any())
                {
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task<int> GetCartDetailsCountAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            return cart?.CartDetails.Count ?? 0;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                _context.CartDetails.RemoveRange(cart.CartDetails);
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
        public async Task PurchaseCartAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.IsPurchased = true;
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
            }
        }
    }
}
