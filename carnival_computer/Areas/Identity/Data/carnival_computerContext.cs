using carnival_computer.Areas.Identity.Data;
using carnival_computer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace carnival_computer.Data;

public class carnival_computerContext : IdentityDbContext<carnival_computerUser>
{
    public carnival_computerContext(DbContextOptions<carnival_computerContext> options)
        : base(options)
    {
    }
    public DbSet<Products> Products { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartDetails> CartDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
