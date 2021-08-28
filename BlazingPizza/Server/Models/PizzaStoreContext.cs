using BlazingPizza.Shared;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Server.Models
{
    public class PizzaStoreContext : ApiAuthorizationDbContext<PizzaStoreUser>
    {
        public DbSet<PizzaSpecial> Specials { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        public PizzaStoreContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PizzaTopping>()
                .HasKey(pst => new { pst.PizzaId, pst.ToppingId });
            modelBuilder.Entity<PizzaTopping>()
                .HasOne<Pizza>().WithMany(ps => ps.Toppings);
            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pst => pst.Topping).WithMany();
            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.DeliveryLocation);

        }
    }
}
