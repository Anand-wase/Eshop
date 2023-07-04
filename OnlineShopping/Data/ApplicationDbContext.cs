using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.WebApi.Models;

namespace OnlineShopping.WebApi.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<CartDetails> CartDetails { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                CategoryName  ="Clothing"    
            },
             new Category
             {
                 CategoryId = 2,
                 CategoryName = "Electronics"
             },
              new Category
              {
                  CategoryId = 3,
                  CategoryName = "Footwear"
              },
               new Category
               {
                   CategoryId = 4,
                   CategoryName = "Groceries"
               },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Mobiles"
                });
    }
}
