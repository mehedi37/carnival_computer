using carnival_computer.Models;

namespace carnival_computer.ViewModel
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartDetails> CartDetails { get; set; } = new List<CartDetails>();
        public decimal TotalPrice { get; set; }
    }
}
