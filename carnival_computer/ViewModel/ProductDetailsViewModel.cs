using carnival_computer.Models;

namespace carnival_computer.ViewModel
{
    public class ProductDetailsViewModel
    {
        public required Products Product { get; set; }
        public required List<Products> OtherProducts { get; set; }
    }
}
