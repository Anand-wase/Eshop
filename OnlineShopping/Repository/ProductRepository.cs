using Microsoft.EntityFrameworkCore;
using OnlineShopping.WebApi.Data;
using OnlineShopping.WebApi.Models;
using OnlineShopping.WebApi.Repository.IRepository;
using System.Linq.Expressions;

namespace OnlineShopping.WebApi.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Product> UpdateAsync(Product entity)
        {
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
