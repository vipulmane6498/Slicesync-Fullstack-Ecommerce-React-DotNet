
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SliceSync.Core.Entities;
using SliceSync.Core.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SliceSync.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Pizza> Pizzas { get; set; }
             public DbSet<Category> Categories{ get; set; }
            public DbSet<PizzaCategoryMapping> PizzaCategoryMappings{ get; set; }

        //OnMoldeCreating -> It's a method where you tell EF Core how to set up your database tables when it can't figure it out automatically.
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            //PizzaCategoryMapping =>  Composite Primary Key (PizzaId + CategoryId together)
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasKey(pc => new { pc.PizzaId, pc.CategoryId });

            //Pizza -> PizzaCategoryMapping
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasOne(pc => pc.Pizza) 
                .WithMany(p => p.pizzaCategoryMapping)
                .HasForeignKey(pc => pc.PizzaId);

            //Category -> PizzaCategoryMapping
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.pizzaCategoryMapping)
                .HasForeignKey(pc => pc.CategoryId);

            //modelbuilder.Entity<Category>()
            //    .Property(c => c.CategoryId)
            //    .HasDefaultValueSql("NEWID()") // ✅ SQL Server generates the Guid
            //.ValueGeneratedOnAdd();
        }
    }
}
