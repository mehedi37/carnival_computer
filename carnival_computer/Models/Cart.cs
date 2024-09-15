using carnival_computer.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carnival_computer.Models
{
    public class Cart
    {
        [Key]
        public required int CartId { get; set; }

        [ForeignKey("carnival_computerUser")]
        public required string UserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public carnival_computerUser? carnival_computerUser { get; set; }

        public ICollection<CartDetails> CartDetails { get; set; } = new List<CartDetails>();

        public bool IsPurchased { get; set; } = false;
    }
}
