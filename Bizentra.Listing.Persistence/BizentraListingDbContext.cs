using Bizentra.Listing.Domain.Common;
using Bizentra.Listing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizentra.Listing.Persistence
{
    public class BizentraListingDbContext : DbContext
    {
        public BizentraListingDbContext(DbContextOptions<BizentraListingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BizentraListingDbContext).Assembly);

            //seed data
            var groceryGuid = Guid.NewGuid();
            var fashionGuid = Guid.NewGuid();
            var gadgetGuid = Guid.NewGuid();
            var techGuid = Guid.NewGuid();
            var cateringGuid = Guid.NewGuid();

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = groceryGuid,
                Name = "Groceries"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = fashionGuid,
                Name = "Fashion"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = gadgetGuid,
                Name = "Gadgets"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = techGuid,
                Name = "Tech Services"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = cateringGuid,
                Name = "Catering Services"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Ofada Rice Big Bag",
                Price = 50,
                Description = "",
                Images = new List<Image>(),
                Business = "MamaGee Foods",
                CategoryId = groceryGuid,
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Fused Denim Pants",
                Price = 40,
                Description = "",
                Images = new List<Image>(),
                Business = "Fused STr",
                Location = "Lagos",
                CategoryId = fashionGuid,
            });
            modelBuilder.Entity<Service>().HasData(new Service
            {
                Id = Guid.NewGuid(),
                Name = "Digital Marketing Services",
                Price = 37,
                Description = "",
                Images = new List<Image>(),
                Business = "Oja Digital Marketer",
                Location = "Ondo",
                CategoryId = techGuid,
            });
            modelBuilder.Entity<Service>().HasData(new Service
            {
                Id = Guid.NewGuid(),
                Name = "Stage Lighting",
                Price = 50,
                Description = "",
                Images = new List<Image>(),
                Business = "Amaka Catering Services",
                Location = "Lagos",
                CategoryId = cateringGuid,
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
