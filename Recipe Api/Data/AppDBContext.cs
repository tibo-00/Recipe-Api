using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Recipe_Api.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Recipes>()
                .HasOne(x => x.Categories)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);

            builder.Entity<Ingredient>()
                .HasOne(x => x.Recipes)
                .WithMany()
                .HasForeignKey(x => x.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Categories>()
                .HasData(
                    new Categories { Id = 1, Name = "Soepen" },
                    new Categories { Id = 2, Name = "Vegetarisch" },
                    new Categories { Id = 3, Name = "Voorgerecht" },
                    new Categories { Id = 4, Name = "Hoofdgerecht" },
                    new Categories { Id = 5, Name = "Dessert" }
                );
        }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Categories> Categories { get; set; }

    }
}
