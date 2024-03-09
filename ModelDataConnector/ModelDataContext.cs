using CbData.Interview.Abstraction.Model;
using Microsoft.EntityFrameworkCore;

namespace CbData.Interview.Common
{
    /// <summary>
    /// Represents an database context for domain model data.
    /// </summary>
    public class ModelDataContext : DbContext
    {
        /// <summary/>
        public ModelDataContext(DbContextOptions<ModelDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Order", "dm");
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().Property(o => o.Id).HasColumnName("OrderId").ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property(o => o.ProductId).HasColumnName("ProductId");
            modelBuilder.Entity<Order>().Property(o => o.Quantity).HasColumnName("Quantity");
        }
    }
}
