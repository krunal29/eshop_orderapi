using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eshop_orderapi.Domain.Models;

namespace eshop_orderapi.Domain
{
    public partial class eshop_orderapiContext : IdentityDbContext<ApplicationUser>
    {
        public eshop_orderapiContext(DbContextOptions<eshop_orderapiContext> options) : base(options)
        {
        }


        public DbSet<Order>Order{ get; set; }

public DbSet<OrderItem>OrderItem{ get; set; }


      
    }
}