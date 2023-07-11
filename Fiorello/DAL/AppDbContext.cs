using Fiorello.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.DAL
{
	public class AppDbContext : IdentityDbContext<User>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

		//Fluent API Look it up
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.HasColumnType("decimal(19,2)");

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}
