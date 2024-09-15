using carnival_computer.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carnival_computer.Models
{
    public class Products
    {
        [Key]
        public required int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public required string ProductImage { get; set; }
        public required decimal ProductPrice { get; set; }
        [ForeignKey("carnival_computerUser")]
        public required string UserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public carnival_computerUser? carnival_computerUser { get; set; }
        public required int Stock { get; set; }
    }
}
