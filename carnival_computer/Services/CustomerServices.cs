using carnival_computer.Data;
using carnival_computer.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace carnival_computer.Services
{
    public class CustomerServices
    {
        private readonly carnival_computerContext _context;

        public CustomerServices(carnival_computerContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerViewModel>> GetCustomersBySellerIdAsync(string sellerId)
        {
            // Fetch the necessary data without performing the aggregation
            var cartData = await _context.Cart
                .Where(c => c.IsPurchased && c.CartDetails.Any(cd => cd.Products.UserId == sellerId))
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Products)
                .Include(c => c.carnival_computerUser)
                .ToListAsync();

            // Perform the aggregation in memory
            var customers = cartData
                .GroupBy(c => c.UserId)
                .Select(g => new CustomerViewModel
                {
                    CustomerName = g.First().carnival_computerUser.Name,
                    TotalSpent = g.Sum(c => c.CartDetails.Sum(cd => cd.Price * cd.Quantity))
                })
                .ToList();

            return customers;
        }
    }
}
