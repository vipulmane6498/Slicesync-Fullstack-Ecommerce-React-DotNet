
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
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
        public DbSet<Category> Categories { get; set; }
        public DbSet<PizzaCategoryMapping> PizzaCategoryMappings { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }



        //OnMoldeCreating -> It's a method where you tell EF Core how to set up your database tables when it can't figure it out automatically.
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            //1. PizzaCategoryMapping----------
            //PizzaCategoryMapping =>  Composite Primary Key (PizzaId + CategoryId together)
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasKey(pc => new { pc.PizzaId, pc.CategoryId });

            //Pizza -> PizzaCategoryMapping
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasOne(pc => pc.Pizza)
                .WithMany(p => p.PizzaCategoryMappings)
                .HasForeignKey(pc => pc.PizzaId);

            //Category -> PizzaCategoryMapping
            modelbuilder.Entity<PizzaCategoryMapping>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.pizzaCategoryMapping)
                .HasForeignKey(pc => pc.CategoryId);


            //2. CartItem----------
            //CartItem=> Cart 
            //We have injected Cart in cartItem Entity so do it like below
            modelbuilder.Entity<CartItem>()
               .HasOne(ci => ci.Cart)
               .WithMany(c => c.CartItems)
               .HasForeignKey(ci => ci.CartId);

            //CartItem=> Pizza
            //We have injected Pizza in cartItem Entity so do it like below
            modelbuilder.Entity<CartItem>()
                .HasOne(ci => ci.Pizza)
                .WithMany() //We did not inject CartItems in Pizza
                .HasForeignKey(ci => ci.PizzaId);

            //3. OrderItem----------
            //OrderItem=> Order
            //We have injected Order in OrderItems Entity so do it like below
            modelbuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            //OrderItem=> Pizza
            //We have injected Pizza in OrderItem Entity so do it like below
            modelbuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Pizza)
                .WithMany() //We did not inject OrderItems in Pizza
                .HasForeignKey(oi => oi.PizzaId);

            //4. Order----------
            //Order=>Application
            modelbuilder.Entity<Order>()
                .HasOne(o => o.ApplicationUser)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            //5. OrderStatusHistory----------
            //OrderStatusHistory -> order
            modelbuilder.Entity<OrderStatusHistory>()
                .HasOne(osh => osh.Order)
                .WithMany(o => o.orderStatusHistories)
                .HasForeignKey(osh => osh.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            //OrderStatusHistory => ApplicationUser
            modelbuilder.Entity<OrderStatusHistory>()
                .HasOne(osh => osh.ApplicationUser)
                .WithMany()
                .HasForeignKey(osh => osh.UserId)
                .OnDelete(DeleteBehavior.NoAction);



        }
    }
}
